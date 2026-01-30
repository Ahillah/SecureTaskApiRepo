using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.RepositoryInterface
{
    public interface IRequestLogRepository
    {
        Task<int> AddRequestLogAsync(RequestLog log);

        Task UpdateResponseLogAsync(RequestLog log);
       Task< RequestLog> GetLog(int Id);

    }
}
