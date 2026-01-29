using Shared.DTO.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto login);
        Task<AuthResponseDto> RegisterAsync(RegisterDto model);

        
    }
}
