using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.IdentityDTO
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string DisplayName { get; set; }

        public string Email { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
