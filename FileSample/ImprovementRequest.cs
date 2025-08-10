using CoreLib.Merges;
using CoreLib.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileSample
{
    public class ImprovementRequest : ICloneable
    {
        public string UserId { get; set; }
        public string RequestDetails { get; set; }
        public DateTime RequestDate { get; set; }

        public ImprovementRequest(string userId, string requestDetails)
        {
            UserId = userId;
            RequestDetails = requestDetails;
            RequestDate = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{RequestDate.ToShortDateString()} - {UserId}: {RequestDetails}";
        }

        public object Clone()
        {
            return new ImprovementRequest(UserId, RequestDetails)
            {
                RequestDate = this.RequestDate
            };
        }
    }

}
