using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = default!;
        public string? ResetPasswordCode { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpiry { get; set; }
        public ICollection<RequestLog> RequestLogs { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } 
    }
}
