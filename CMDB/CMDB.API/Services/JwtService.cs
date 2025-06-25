using CMDB.API.Helper;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMDB.API.Services
{
    /// <summary>
    /// This class is used to generate JWT token
    /// </summary>
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        private JwtService()
        {
            
        }
        /// <summary>
        /// This constructor is used to inject the configuration
        /// </summary>
        /// <param name="configuration"></param>
        public JwtService(IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
        }
        /// <summary>
        /// This function will generate the JWT token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateToken(Admin user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.PrimarySid, user.AdminId.ToString()),
                new(ClaimTypes.UserData, user.Account.UserID),
                new(ClaimTypes.Name, user.Level.ToString()),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryInMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
