using BevCapital.Logon.Application.Gateways.Security;
using BevCapital.Logon.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BevCapital.Logon.Infra.Security
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly ITokenSecret _tokenSecret;

        public TokenGenerator(ITokenSecret tokenSecret)
        {
            _tokenSecret = tokenSecret;
        }

        public async Task<string> CreateToken(AppUser appUser)
        {
            var secretKey = await _tokenSecret.GetSecretAsync();
            if (string.IsNullOrWhiteSpace(secretKey))
                return secretKey;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new List<Claim>
             {
                 new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                 new Claim(JwtRegisteredClaimNames.GivenName, appUser.Name)
             };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
