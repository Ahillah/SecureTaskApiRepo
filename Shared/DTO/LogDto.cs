using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class LogDto
    { public int id { get;set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public string IpAddress { get; set; }
        public int StatusCode { get; set; }
        public string ResponseTime { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
        public string Headers { get; set; }

    }
}
