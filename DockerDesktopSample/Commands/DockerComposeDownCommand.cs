using DockerDesktop.Models;
using DockerDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Commands
{
    public class DockerComposeDownCommand
    {
        private readonly IDockerWslCommandService _dockerService;
        private readonly string _composeFilePath;

        public DockerComposeDownCommand(IDockerWslCommandService service, string composeFilePath)
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
            return $"docker compose -f {_composeFilePath} down";
        }
    }
}
