using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Models
{
    public class CommandOptions
    {
        public string WorkingDirectory { get; set; } = Environment.CurrentDirectory;
        public int TimeoutMilliseconds { get; set; } = 30000; // 30秒
        public bool ShowWindow { get; set; } = false;
        public bool UseShellExecute { get; set; } = false;
    }
}
