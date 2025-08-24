using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerDesktop.Models
{
    public class DockerContainer
    {
        public string ContainerId { get; set; }
        public string Image { get; set; }
        public string Command { get; set; }
        private string _created;
        public string Created
        {
            get => _created;
            set
            {
                _created = value;
                // "2025-08-22 19:34:26 +0900 JST" の形式をパース
                if (!string.IsNullOrWhiteSpace(value))
                {
                    // JST部分を除去してパース
                    var trimmed = value.Substring(0, value.LastIndexOf(' '));
                    if (DateTimeOffset.TryParseExact(
                            trimmed,
                            "yyyy-MM-dd HH:mm:ss zzz",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None,
                            out var dto))
                    {
                        CreatedAt = dto.DateTime;
                    }
                    else
                    {
                        CreatedAt = default;
                    }
                }
                else
                {
                    CreatedAt = default;
                }
            }
        }
        public string Status { get; set; }
        public string Ports { get; set; }
        public string Names { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
