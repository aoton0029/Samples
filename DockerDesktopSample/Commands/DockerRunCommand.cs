using DockerDesktop.Models;
using DockerDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Commands
{
    public class DockerRunCommand : IDockerCommand
    {
        private readonly IDockerWslCommandService _dockerService;
        private readonly string _imageName;
        private readonly string[] _options;

        public DockerRunCommand(IDockerWslCommandService service, string imageName, params string[] options)
        {
            _dockerService = service ?? throw new ArgumentNullException(nameof(service));
            _imageName = imageName ?? throw new ArgumentNullException(nameof(imageName));
            _options = options ?? Array.Empty<string>();
        }

        public async Task<CommandResult> ExecuteAsync()
        {
            return await _dockerService.ExecuteAsync(
                GetCommandString(),
                new CommandOptions { ShowWindow = false }
            );
        }

        public string GetCommandString()
        {
            var optionsString = string.Join(" ", _options);
            return $"docker run {optionsString} {_imageName}".Trim();
        }

        public string BuildRunCommand(string image, DockerRunOptions options)
        {
            var commands = new List<string> { "docker run" };

            if (options.Detach) commands.Add("-d");
            if (options.Interactive) commands.Add("-i");
            if (options.Tty) commands.Add("-t");
            if (options.RemoveOnExit) commands.Add("--rm");

            if (!string.IsNullOrEmpty(options.Name))
                commands.Add($"--name {options.Name}");

            if (!string.IsNullOrEmpty(options.Network))
                commands.Add($"--network {options.Network}");

            if (!string.IsNullOrEmpty(options.WorkingDirectory))
                commands.Add($"-w {options.WorkingDirectory}");

            if (!string.IsNullOrEmpty(options.User))
                commands.Add($"-u {options.User}");

            // 環境変数
            foreach (var env in options.Environment)
            {
                commands.Add($"-e {env.Key}={env.Value}");
            }

            // ボリューム
            foreach (var volume in options.Volumes)
            {
                commands.Add($"-v {volume.Key}:{volume.Value}");
            }

            // ポート
            foreach (var port in options.Ports)
            {
                commands.Add($"-p {port.Key}:{port.Value}");
            }

            commands.Add(image);

            if (options.Command.Any())
            {
                commands.AddRange(options.Command);
            }

            return string.Join(" ", commands);
        }

    }
}
