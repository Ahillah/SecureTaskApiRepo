using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

        public RequestLoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
           
        
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
         
            var stopwatch = Stopwatch.StartNew();
            var userId =
                context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var headersJson = JsonSerializer.Serialize(
                context.Request.Headers
                .Where(h => h.Key != "Authorization")
                .ToDictionary(h => h.Key, h => h.Value.ToString())
            );

            using (var scope = _scopeFactory.CreateScope())
            {
                var logService = scope.ServiceProvider.GetRequiredService<IRequestLoggingService>();
                var logDto = new LogDto
                {
                    HttpMethod = context.Request.Method,
                    Url = context.Request.Path,
                    IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                    Headers = headersJson,
                    UserId = userId,
                    Timestamp = DateTime.UtcNow,
                      StatusCode = 0,                
                    ResponseTime = "0 ms"
                };


                await logService.LogRequestAsync(logDto);
                await _next(context);




                stopwatch.Stop();
                logDto.StatusCode = context.Response.StatusCode;
                logDto.ResponseTime = $"{stopwatch.ElapsedMilliseconds} ms";

                await logService.LogResponseAsync(logDto);
            }
        }


       
    }
}