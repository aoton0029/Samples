using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Models
{
    public class DockerImage
    {
        public string Repository { get; set; }
        public string Tag { get; set; }
        public string ImageId { get; set; }
        public string Created { get; set; }
        public string Size { get; set; }
    }
}
