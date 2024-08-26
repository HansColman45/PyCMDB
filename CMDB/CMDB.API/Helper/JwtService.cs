using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMDB.API.Helper
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly CMDBContext _CMDBContext;

        public JwtService(IConfiguration configuration, CMDBContext context)
        {
            _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
            _CMDBContext = context;
        }
        /// <summary>
        /// 
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
            var menus = _CMDBContext.RolePerms
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
