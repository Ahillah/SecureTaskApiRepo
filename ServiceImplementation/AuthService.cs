using DomainLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DTO.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace ServiceImplementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public AuthService(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto login)
        {
            var user = await userManager.FindByEmailAsync(login.Email);
            bool isValid = false;

            if (user != null)
            {
                isValid = await userManager.CheckPasswordAsync(user, login.Password);
            }

            if (!isValid)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Invalid email or password." }
                };
            }




            return new AuthResponseDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                IsSuccess = true,
                Token = await CreateTokenAsync(user),
               
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
        {
            var User = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                DisplayName = model.Name,

               


            };
            var Result = await userManager.CreateAsync(User, model.Password);

            if (Result.Succeeded)
            {
                return new AuthResponseDto()
                {
                    IsSuccess = true,
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token = await CreateTokenAsync(User),
                 
                };


            }
            else
            {
                return new AuthResponseDto()
                {
                    IsSuccess = false,
                    Errors = Result.Errors.Select(e => e.Description).ToList()

                };
            }
        }

        public async Task<string> CreateTokenAsync(User user)
        {
            var claim = new List<Claim>()
            {
              new Claim (ClaimTypes.Email, user.Email!),
              new Claim(ClaimTypes.Name,user.DisplayName!),
              new Claim(ClaimTypes.NameIdentifier,user.Id!),
              new Claim("UserId", user.Id)

            };
           
            var secretKey = configuration.GetSection("JwtOptions")["SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
               issuer: configuration.GetSection("JwtOptions")["Issuer"],
               audience: configuration.GetSection("JwtOptions")["Audience"],
               signingCredentials: Creds,
               expires: DateTime.Now.AddHours(1),
               claims: claim


                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }




}
    }