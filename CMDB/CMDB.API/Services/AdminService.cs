using CMDB.API.Helper;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class AdminService : LogService, IAdminService
    {
        private readonly JwtService _jwtService;
        private readonly CMDBContext _context;

        public AdminService(JwtService jwtService, CMDBContext context): base(context)
        {
            _jwtService = jwtService;
            _context = context;
        }
        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var admin = await _context.Admins
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Where(x => x.Account.Application.Name == "CMDB" && x.Account.UserID == model.Username).FirstOrDefaultAsync();
            if (admin == null)
                return null;

            if (string.Equals(admin.Password, new PasswordHasher().EncryptPassword(model.Password))) { 
                var token = _jwtService.GenerateToken(admin);
                return new AuthenticateResponse(admin, token);
            }
            else
                return null;
        }
        public async Task<IEnumerable<AdminDetailResponce>> GetAll()
        {
            var list = await _context.Admins
                .Include(x => x.Accounts)
                .Select(x => new { x.AdminId, x.Account.UserID, x.Level, x.active})
                .ToListAsync();
            List<AdminDetailResponce> admins = new();
            foreach (var account in list)
            {
                admins.Add(new()
                {
                    AdminId = account.AdminId,
                    Active = account.active == 1 ? "Active":"Deacive",
                    Level = account.Level,
                    UserId = account.UserID
                });
            }
            return admins;
        }
        public async Task<Admin?> GetById(int id)
        {
            return await _context.Admins
                .Where(x => x.AdminId == id).FirstOrDefaultAsync();
        }
        public Task Create(Admin admin)
        {
            throw new NotImplementedException();
        }
        public Task Update(Admin admin)
        {
            throw new NotImplementedException();
        }
    }
}
