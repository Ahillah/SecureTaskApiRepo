using DomainLayer;
using DomainLayer.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using ServiceAbstraction;
using Shared.DTO;
using Shared.DTO.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation
{

    public class RequestLoggingService : IRequestLoggingService
    {
        private readonly IRequestLogRepository _repo;

        public RequestLoggingService(IRequestLogRepository repo)
        {
            _repo = repo;
        }

        public async Task LogRequestAsync(LogDto logDto)
        {
            var log = new RequestLog
            {
                HttpMethod = logDto.HttpMethod,
                Url = logDto.Url,
                IpAddress = logDto.IpAddress,
                Timestamp = logDto.Timestamp,
                ResponseTime = logDto.ResponseTime,
                StatusCode = logDto.StatusCode,
                UserId = logDto.UserId,
                Headers= logDto.Headers,
                
                    
            };
            logDto.id = await _repo.AddRequestLogAsync(log);
            

        }



        public async Task LogResponseAsync(LogDto logDto)
        {
            var log = await _repo.GetLog(logDto.id);
            if (log != null)
            {
                log.StatusCode = logDto.StatusCode;
                log.ResponseTime = logDto.ResponseTime;
                log.UserId = logDto.UserId;
                

                await _repo.UpdateResponseLogAsync(log);
            }
        }
    }
}

      
