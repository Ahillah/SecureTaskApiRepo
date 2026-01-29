
using DomainLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistance;
using ServiceAbstraction;
using ServiceImplementation;
using System.Text;



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



            app.MapControllers();

            app.Run();
        }
    }
}
