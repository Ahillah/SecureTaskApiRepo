using DomainLayer;
using DomainLayer.RepositoryInterface;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.RepositoryImplementation
{
    public class RequestLogRepository : IRequestLogRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RequestLogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> AddRequestLogAsync(RequestLog log)
        {
            await _dbContext.RequestLogs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
            return log.Id;
        }

        public async Task<RequestLog> GetLog(int Id)
        {
           return await _dbContext.RequestLogs.FindAsync(Id);
        }

        public async Task UpdateResponseLogAsync(RequestLog log)
        {
            _dbContext.RequestLogs.Update(log);
            await _dbContext.SaveChangesAsync(); ;
        }
    }
}
