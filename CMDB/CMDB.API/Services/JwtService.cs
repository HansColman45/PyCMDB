using CMDB.API.Helper;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMDB.API.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
        }
        /// <summary>
        /// This function will generate the JWT token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateToken(CMDBContext context, Admin user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.PrimarySid, user.AdminId.ToString()),
                new(ClaimTypes.UserData, user.Account.UserID),
                new(ClaimTypes.Name, user.Level.ToString()),
            };
            var menus = context.RolePerms
                .AsNoTracking()
                .Include(x => x.Menu)
                .Include(x => x.Permission)
                .Where(x => x.Level == user.Level)
                .Select(x => new { x.Menu.Label, x.Permission.Rights })
                .ToList();
            foreach (var menu in menus)
            {
                claims.Add(new(ClaimTypes.Role, menu.Label));
                claims.Add(new(ClaimTypes.NameIdentifier, menu.Rights));
            }
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
