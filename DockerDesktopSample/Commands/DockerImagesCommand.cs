using DockerDesktop.Models;
using DockerDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Commands
{
    public class DockerImagesCommand : IDockerCommand
    {
        private readonly IDockerWslCommandService _dockerService;

        public DockerImagesCommand(IDockerWslCommandService service)
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
            return "docker images --format 'table {{.Repository}}\a{{.Tag}}\a{{.ID}}\a{{.CreatedAt}}\a{{.Size}}'";
        }

        public DockerImage[] ParseImageOutput(string output)
        {
            var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var images = new List<DockerImage>();

            foreach (var line in lines)
            {
                var parts = line.Split('\a');
                if (parts.Length >= 5)
                {
                    images.Add(new DockerImage
                    {
                        Repository = parts[0],
                        Tag = parts[1],
                        ImageId = parts[2],
                        Created = parts[3],
                        Size = parts[4]
                    });
                }
            }

            return images.ToArray();
        }

    }
}
