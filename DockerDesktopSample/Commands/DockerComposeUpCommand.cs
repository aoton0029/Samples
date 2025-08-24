using DockerDesktop.Models;
using DockerDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Commands
{
    internal class DockerComposeUpCommand : IDockerCommand
    {
        private readonly IDockerWslCommandService _dockerService;
        private readonly string _composeFilePath;
        public DockerComposeUpCommand(IDockerWslCommandService service, string composeFilePath)
        {
            _dockerService = service ?? throw new ArgumentNullException(nameof(service));
            _composeFilePath = composeFilePath ?? throw new ArgumentNullException(nameof(composeFilePath));
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
            return $"docker compose up -f {_composeFilePath} -d";
        }

    }
}
