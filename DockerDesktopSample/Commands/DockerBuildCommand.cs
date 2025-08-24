using DockerDesktop.Models;
using DockerDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Commands
{
    public class DockerBuildCommand : IDockerCommand
    {
        private readonly IDockerWslCommandService _dockerService;

        public DockerBuildCommand(IDockerWslCommandService service)
        {
            _dockerService = service ?? throw new ArgumentNullException(nameof(service));
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
            return "docker build";
        }

        public string BuildBuildCommand(string imageName, DockerBuildOptions options)
        {
            var commands = new List<string> { "docker build" };

            if (options.NoCache) commands.Add("--no-cache");
            if (options.Quiet) commands.Add("-q");

            if (!string.IsNullOrEmpty(options.Platform))
                commands.Add($"--platform {options.Platform}");

            if (!string.IsNullOrEmpty(options.Dockerfile))
                commands.Add($"-f {options.Dockerfile}");

            // Build Args
            foreach (var arg in options.BuildArgs)
            {
                commands.Add($"--build-arg {arg.Key}={arg.Value}");
            }

            // Tags
            if (options.Tags.Any())
            {
                foreach (var tag in options.Tags)
                {
                    commands.Add($"-t {tag}");
                }
            }
            else
            {
                commands.Add($"-t {imageName}");
            }

            commands.Add(options.BuildContext);

            return string.Join(" ", commands);
        }

    }
}
