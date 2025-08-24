using DockerDesktop.Commands;
using DockerDesktop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockerDesktop.Services
{
    public class DockerWslCommandService : IDockerWslCommandService
    {
        public event EventHandler<string> OutputReceived;
        public event EventHandler<string> ErrorReceived;
        public event EventHandler<CommandResult> CommandCompleted;
        private readonly string _wslDistribution;

        public DockerWslCommandService(string wslDistribution = "Ubuntu")
        {
            _wslDistribution = wslDistribution;
        }

        public async Task<CommandResult> ExecuteAsync(
            string command
            , CommandOptions options = null
            , CancellationToken cancellationToken = default)
        {
            options = options ?? new CommandOptions();
            var stopwatch = Stopwatch.StartNew();

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            var psi = new ProcessStartInfo
            {
                FileName = "wsl",
                Arguments = $"-d {_wslDistribution} {command}",
                WorkingDirectory = options.WorkingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = !options.ShowWindow
            };
            Debug.Print(psi.Arguments);
            try
            {
                using var process = new Process { StartInfo = psi };

                // イベントハンドラの設定
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        outputBuilder.AppendLine(e.Data);
                        OutputReceived?.Invoke(this, e.Data);
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        errorBuilder.AppendLine(e.Data);
                        ErrorReceived?.Invoke(this, e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // タイムアウトとキャンセル対応
                var timeoutTask = Task.Delay(options.TimeoutMilliseconds, cancellationToken);
                var processTask = Task.Run(() => process.WaitForExit(), cancellationToken);

                var completedTask = await Task.WhenAny(processTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    process.Kill();
                    var message = cancellationToken.IsCancellationRequested
                        ? "Command was cancelled"
                        : $"Command timed out after {options.TimeoutMilliseconds}ms";
                    throw new OperationCanceledException(message);
                }

                stopwatch.Stop();

                var result = new CommandResult
                {
                    ExitCode = process.ExitCode,
                    StandardOutput = outputBuilder.ToString(),
                    StandardError = errorBuilder.ToString(),
                    ExecutionTime = stopwatch.Elapsed
                };

                CommandCompleted?.Invoke(this, result);
                return result;
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                stopwatch.Stop();
                Debug.Print($"Failed to execute command: {command}");
                throw new InvalidOperationException($"Failed to execute command: {command}", ex);
            }
        }
    }
}
