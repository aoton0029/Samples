using CoreLib.Cmds;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktopSample
{
    class DockerWslUsageExample
    {
        public static async Task Main(string[] args)
        {
            // WSLオプションの設定（必要に応じて）
            var wslOptions = new WslCommandExecutor.WslCommandOptions
            {
                Distribution = "Ubuntu", // 使用するWSLディストリビューション
                User = "notoa", // WSLユーザー名
                WslWorkingDirectory = "/home/notoa/projects" // 作業ディレクトリ
            };

            var dockerExecutor = new DockerWslExecutor(wslOptions);

            // リアルタイム出力の設定
            dockerExecutor.OutputReceived += (sender, output) =>
                Debug.WriteLine($"[DOCKER OUT] {output}");
            dockerExecutor.ErrorReceived += (sender, error) =>
                Debug.WriteLine($"[DOCKER ERR] {error}");

            try
            {
                // Dockerが利用可能かチェック
                Debug.WriteLine("=== Docker Availability Check ===");
                var isAvailable = await dockerExecutor.IsDockerAvailableAsync();
                Debug.WriteLine($"Docker Available: {isAvailable}");

                if (!isAvailable)
                {
                    Debug.WriteLine("Docker is not available in WSL. Please install Docker first.");
                    return;
                }

                // Dockerバージョン情報
                Debug.WriteLine("\n=== Docker Version ===");
                var versionResult = await dockerExecutor.GetDockerVersionAsync();
                Debug.WriteLine(versionResult.StandardOutput);

                // 現在のコンテナ一覧
                Debug.WriteLine("\n=== Current Containers ===");
                var containers = await dockerExecutor.GetContainersAsync(includeAll: true);
                foreach (var container in containers)
                {
                    Debug.WriteLine($"ID: {container.ContainerId}, Image: {container.Image}, " +
                                    $"Status: {container.Status}, Name: {container.Names}");
                }

                // イメージ一覧
                Debug.WriteLine("\n=== Images ===");
                var images = await dockerExecutor.GetImagesAsync();
                foreach (var image in images)
                {
                    Debug.WriteLine($"Repository: {image.Repository}, Tag: {image.Tag}, " +
                                    $"Size: {image.Size}");
                }

                // Hello Worldコンテナを実行
                Debug.WriteLine("\n=== Running Hello World Container ===");
                var runOptions = new DockerWslExecutor.DockerRunOptions
                {
                    RemoveOnExit = true, // 実行後に自動削除
                    Interactive = false,
                    Tty = false
                };

                var runResult = await dockerExecutor.RunContainerAsync("hello-world", runOptions);
                Debug.WriteLine($"Run Result Success: {runResult.IsSuccess}");
                if (runResult.IsSuccess)
                {
                    Debug.WriteLine("Hello World Output:");
                    Debug.WriteLine(runResult.StandardOutput);
                }

                // Nginx コンテナをデタッチモードで実行
                Debug.WriteLine("\n=== Running Nginx Container (Detached) ===");
                var nginxOptions = new DockerWslExecutor.DockerRunOptions
                {
                    Detach = true,
                    Name = "my-nginx",
                    Ports = new Dictionary<int, int> { { 8080, 80 } },
                    Environment = new Dictionary<string, string>
                {
                    { "NGINX_HOST", "localhost" },
                    { "NGINX_PORT", "80" }
                }
                };

                var nginxResult = await dockerExecutor.RunContainerAsync("nginx:alpine", nginxOptions);
                Debug.WriteLine($"Nginx Container Started: {nginxResult.IsSuccess}");

                if (nginxResult.IsSuccess)
                {
                    // 少し待ってからログを確認
                    await Task.Delay(3000);

                    Debug.WriteLine("\n=== Nginx Container Logs ===");
                    var logsResult = await dockerExecutor.GetContainerLogsAsync("my-nginx", tailLines: 10);
                    Debug.WriteLine(logsResult.StandardOutput);

                    // コンテナでコマンドを実行
                    Debug.WriteLine("\n=== Executing Command in Nginx Container ===");
                    var execResult = await dockerExecutor.ExecInContainerAsync("my-nginx", "nginx -v");
                    Debug.WriteLine($"Nginx Version: {execResult.StandardOutput}");

                    // コンテナを停止して削除
                    Debug.WriteLine("\n=== Stopping and Removing Nginx Container ===");
                    await dockerExecutor.StopContainerAsync("my-nginx");
                    await dockerExecutor.RemoveContainerAsync("my-nginx");
                    Debug.WriteLine("Nginx container stopped and removed.");
                }

                // Docker Composeの例（docker-compose.ymlがある場合）
                Debug.WriteLine("\n=== Docker Compose Example ===");
                try
                {
                    // Docker Composeファイルが存在する場合のみ実行
                    var composeResult = await dockerExecutor.ComposeUpAsync("docker-compose.yml", detach: true);
                    if (composeResult.IsSuccess)
                    {
                        Debug.WriteLine("Docker Compose services started successfully.");

                        // 少し待ってから停止
                        await Task.Delay(5000);
                        await dockerExecutor.ComposeDownAsync("docker-compose.yml");
                        Debug.WriteLine("Docker Compose services stopped.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Docker Compose operation failed: {ex.Message}");
                }

                // システムの使用量を表示
                Debug.WriteLine("\n=== Docker System Usage ===");
                var usageResult = await dockerExecutor.GetSystemUsageAsync();
                Debug.WriteLine(usageResult.StandardOutput);

                // 必要に応じてシステムクリーンアップ
                Debug.WriteLine("\n=== System Cleanup (if needed) ===");
                Debug.WriteLine("Skipping cleanup for safety. Use PruneSystemAsync() if needed.");

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            Debug.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    class WslUsageExample
    {
        static async Task Main(string[] args)
        {
            var wslExecutor = new WslCommandExecutor();

            // リアルタイム出力の設定
            wslExecutor.OutputReceived += (sender, output) => Debug.WriteLine($"[WSL OUT] {output}");
            wslExecutor.ErrorReceived += (sender, error) => Debug.WriteLine($"[WSL ERR] {error}");

            try
            {
                // WSLディストリビューション一覧を表示
                Debug.WriteLine("=== WSL Distributions ===");
                var distributions = await wslExecutor.GetDistributionsAsync();
                foreach (var dist in distributions)
                {
                    Debug.WriteLine($"Name: {dist.Name}, State: {dist.State}, Version: {dist.Version}, Default: {dist.IsDefault}");
                }

                // 基本的なコマンド実行
                Debug.WriteLine("\n=== Basic Command ===");
                var result1 = await wslExecutor.ExecuteWslCommandAsync("pwd && whoami");
                Debug.WriteLine($"Current Directory and User:\n{result1.StandardOutput}");

                // 特定のディストリビューションで実行
                Debug.WriteLine("\n=== Distribution Specific ===");
                var options = new WslCommandExecutor.WslCommandOptions
                {
                    Distribution = "Ubuntu-20.04", // 存在する場合
                    WslWorkingDirectory = "/home/ubuntu",
                    User = "ubuntu",
                    Environment = new Dictionary<string, string>
                {
                    { "MY_VAR", "Hello from C#" },
                    { "PATH", "/usr/local/bin:$PATH" }
                }
                };

                var result2 = await wslExecutor.ExecuteWslCommandAsync("echo $MY_VAR && pwd", options);
                if (result2.IsSuccess)
                {
                    Debug.WriteLine($"Environment Variable Result:\n{result2.StandardOutput}");
                }

                // Bashスクリプトの実行
                Debug.WriteLine("\n=== Bash Script ===");
                var bashScript = @"#!/bin/bash
echo ""Starting script...""
for i in {1..3}; do
    echo ""Step $i""
    sleep 1
done
echo ""Script completed!""
";

                var scriptResult = await wslExecutor.ExecuteBashScriptAsync(bashScript, options);
                Debug.WriteLine($"Script Result:\n{scriptResult.StandardOutput}");

                // バッチコマンド実行
                Debug.WriteLine("\n=== Batch Commands ===");
                string[] commands = {
                "echo 'Command 1'",
                "ls -la /tmp",
                "echo 'Command 3'",
                "uname -a"
            };

                var batchResults = await wslExecutor.ExecuteWslBatchAsync(commands, options);
                for (int i = 0; i < batchResults.Length; i++)
                {
                    Debug.WriteLine($"Command {i + 1} Success: {batchResults[i].IsSuccess}");
                }

                // パス変換の例
                Debug.WriteLine("\n=== Path Conversion ===");
                try
                {
                    var windowsPath = @"C:\Windows\System32";
                    var wslPath = await wslExecutor.ConvertWindowsPathToWslAsync(windowsPath);
                    Debug.WriteLine($"Windows Path: {windowsPath}");
                    Debug.WriteLine($"WSL Path: {wslPath}");

                    var backToWindows = await wslExecutor.ConvertWslPathToWindowsAsync(wslPath);
                    Debug.WriteLine($"Back to Windows: {backToWindows}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Path conversion error: {ex.Message}");
                }

                // ファイル操作の例
                Debug.WriteLine("\n=== File Operations ===");
                var tempFile = Path.GetTempFileName();
                File.WriteAllText(tempFile, "Hello from Windows!");

                try
                {
                    var copyResult = await wslExecutor.CopyFileToWslAsync(
                        tempFile,
                        "/tmp/windows_file.txt",
                        options);

                    if (copyResult.IsSuccess)
                    {
                        var catResult = await wslExecutor.ExecuteWslCommandAsync("cat /tmp/windows_file.txt", options);
                        Debug.WriteLine($"File content in WSL: {catResult.StandardOutput}");
                    }
                }
                finally
                {
                    File.Delete(tempFile);
                    await wslExecutor.ExecuteWslCommandAsync("rm -f /tmp/windows_file.txt", options);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }

            Debug.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
