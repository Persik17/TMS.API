using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using TMS.Application.Dto.User;
using TMS.Application.Abstractions.Security;

namespace TMS.Application.Security
{
    public class JwtTokenService : ITokenService
    {
        private readonly string _jwtSecret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _lifetimeMinutes;

        public JwtTokenService(IConfiguration config)
        {
            _jwtSecret = config["Jwt:Secret"];
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
            _lifetimeMinutes = int.Parse(config["Jwt:LifetimeMinutes"] ?? "60");
        }

        public string GenerateToken(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new("userId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_lifetimeMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token, out Guid userId)
        {
            userId = Guid.Empty;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)),
                    ValidateLifetime = true,
                };

                var principal = handler.ValidateToken(token, parameters, out var securityToken);
                var idClaim = principal.Claims.FirstOrDefault(c => c.Type == "userId");
                if (idClaim != null && Guid.TryParse(idClaim.Value, out var id))
                {
                    userId = id;
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
    }
}