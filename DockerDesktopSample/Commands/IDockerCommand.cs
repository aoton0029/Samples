using DockerDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Commands
{
    public interface IDockerCommand
    {
        Task<CommandResult> ExecuteAsync();
        string GetCommandString();
    }
}
