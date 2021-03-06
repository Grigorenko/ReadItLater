using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ReadItLater.Web.Server.Utils
{
    public static class AuthenticationHelper
    {
        public static string GenerateJwtToken(string userId, string secret, int jwtTokenExpirationTimeInMinutes = 2)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwtTokenExpirationTimeInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static (string Token, DateTime Expires, DateTime Created, string ipAddress) GenerateRefreshToken(string ipAddress, int refreshTokenExpirationTimeInHours = 168)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);

                return (Convert.ToBase64String(randomBytes), DateTime.UtcNow.AddHours(refreshTokenExpirationTimeInHours), DateTime.UtcNow, ipAddress);
            }
        }
    }
}
