using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Roulette.Shared.Rows;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Roulette.Application.SignInManager
{
    public class SignInManager : ISignInManager
    {
        private readonly TokenSettings _tokenSettings;

        public SignInManager(IOptions<TokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
        }

        public string GetPassword(string flatPassword, string salt)
        {
            string inputString = $"{flatPassword}{salt}";

            var crypter = new SHA256Managed();
            var stringBuilder = new StringBuilder();

            byte[] crypto = crypter.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            foreach (byte theByte in crypto)
                stringBuilder.Append(theByte.ToString("x2"));

            return stringBuilder.ToString();
        }

        public string GenerateToken()
        {
            var claim = new[]
            {
                new Claim(ClaimTypes.Name, "sample thing")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _tokenSettings.Issuer,
                _tokenSettings.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(_tokenSettings.AccessExpiration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
