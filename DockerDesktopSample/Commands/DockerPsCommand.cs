using DockerDesktop.Models;
using DockerDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.DataFormats;

namespace DockerDesktop.Commands
{
    public class DockerPsCommand : IDockerCommand
    {
        private readonly IDockerWslCommandService _dockerService;
        private readonly bool _all;

        public DockerPsCommand(IDockerWslCommandService service, bool all = false)
        {
            _dockerService = service ?? throw new ArgumentNullException(nameof(service));
            _all = all;
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
            string ret = _all ? "docker ps -a" : "docker ps";
            ret += " --format 'table {{.ID}}\a{{.Image}}\a{{.Command}}\a{{.CreatedAt}}\a{{.Status}}\a{{.Ports}}\a{{.Names}}'";
            return ret;
        }

        public DockerContainer[] ParseContainerOutput(string output)
        {
            var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var containers = new List<DockerContainer>();

            foreach (var line in lines)
            {
                var parts = line.Split('\a');
                if (parts.Length >= 7)
                {
                    containers.Add(new DockerContainer
                    {
                        ContainerId = parts[0],
                        Image = parts[1],
                        Command = parts[2],
                        Created = parts[3],
                        Status = parts[4],
                        Ports = parts[5],
                        Names = parts[6]
                    });
                }
            }

            return containers.ToArray();
        }
    }
}
