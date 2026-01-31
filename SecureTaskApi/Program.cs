
using DomainLayer;
using DomainLayer.RepositoryInterface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistance;
using Persistance.RepositoryImplementation;
using Presentation.Middleware;
using ServiceAbstraction;
using ServiceImplementation;
using System.Text;
using System.Threading.RateLimiting;



namespace SecureTaskApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("BaseConnection"),
                    b => b.MigrationsAssembly("Persistance")
                );
            });

            builder.Services.AddAuthentication(
               config =>
               {
                   config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

               }
               ).AddJwtBearer(
               options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = true,
                       ValidIssuer = builder.Configuration["JwtOptions:Issuer"],

                       ValidateAudience = true,
                       ValidAudience = builder.Configuration["JwtOptions:Audience"],

                       ValidateLifetime = true,

                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(
         Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"])
     )
                   };



               }
               );
            builder.Services
         .AddIdentityCore<User>()
         .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    string key = httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress.ToString();

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: key,
                        partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        });
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
            builder.Services.AddScoped<IRequestLogRepository, RequestLogRepository>();
            builder.Services.AddScoped<IRequestLoggingService, RequestLoggingService>();
            builder.Services.AddScoped<ITaskRepository,TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();


            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
         
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
          
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseRateLimiter();
            app.MapControllers();

            app.Run();
        }
    }
}
