using ETicaret.Application.Abstractions.Token;
using ETicaret.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int second, AppUser appUser)
        {
            Application.DTOs.Token token = new();
            
            // security key simetriğini alıyoruz
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            // şifrelenmiş kimliği oluşturuyoruz.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            // token ayarları
            token.Expiration = DateTime.UtcNow.AddSeconds(second);

            JwtSecurityToken jwtSecurityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires:token.Expiration,
                notBefore:DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: new Claim[]
                {
                   new Claim(ClaimTypes.Name,appUser.UserName)
                }
                );

            //token oluşturucu sınıfından örnek alma
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);
            
            token.RefreshToken = CreateRefreshToken();

            return token;   
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
