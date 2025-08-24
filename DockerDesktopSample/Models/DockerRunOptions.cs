using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Models
{
    public class DockerRunOptions
    {
        public bool Detach { get; set; } = false;
        public bool Interactive { get; set; } = false;
        public bool Tty { get; set; } = false;
        public string Name { get; set; }
        public Dictionary<string, string> Environment { get; set; } = new();
        public Dictionary<string, string> Volumes { get; set; } = new();
        public Dictionary<int, int> Ports { get; set; } = new();
        public string Network { get; set; }
        public string WorkingDirectory { get; set; }
        public string User { get; set; }
        public bool RemoveOnExit { get; set; } = false;
        public string[] Command { get; set; } = Array.Empty<string>();
    }
}
