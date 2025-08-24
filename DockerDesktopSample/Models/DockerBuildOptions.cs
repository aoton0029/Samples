using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Models
{
    public class DockerBuildOptions
    {
        public string Dockerfile { get; set; } = "Dockerfile";
        public string BuildContext { get; set; } = ".";
        public Dictionary<string, string> BuildArgs { get; set; } = new();
        public string[] Tags { get; set; } = Array.Empty<string>();
        public bool NoCache { get; set; } = false;
        public string Platform { get; set; }
        public bool Quiet { get; set; } = false;
    }
}
