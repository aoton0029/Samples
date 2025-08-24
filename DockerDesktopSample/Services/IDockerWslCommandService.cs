using DockerDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Services
{
    public interface IDockerWslCommandService
    {
        event EventHandler<string> OutputReceived;
        event EventHandler<string> ErrorReceived;
        event EventHandler<CommandResult> CommandCompleted;
        Task<CommandResult> ExecuteAsync(
            string command
            , CommandOptions options = null
            , CancellationToken cancellationToken = default);
    }
}
