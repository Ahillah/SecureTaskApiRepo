using Shared.DTO;
using Shared.DTO.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IRequestLoggingService
    {
        Task LogRequestAsync(LogDto log);
        Task LogResponseAsync(LogDto log);
    }
}
