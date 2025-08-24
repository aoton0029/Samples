using DockerDesktop.Models;
using DockerDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Commands
{
    public class DockerStopCommand : IDockerCommand
    {
        private readonly IDockerWslCommandService _dockerService;
        private readonly string _containerIdOrName;

        public DockerStopCommand(IDockerWslCommandService service, string containerIdOrName)
        {
            _dockerService = service ?? throw new ArgumentNullException(nameof(service));
            _containerIdOrName = containerIdOrName ?? throw new ArgumentNullException(nameof(containerIdOrName));
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
            return $"docker stop {_containerIdOrName}";
        }
    }
}
