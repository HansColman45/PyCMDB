using CMDB.API.Helper;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class AdminService : CMDBService, IAdminService
    {
        private readonly JwtService _jwtService;
        private readonly string Table = "admin"; 
        private readonly ILogger<AdminService> _logger;
        private ILogService _logService;
        public AdminService(JwtService jwtService, CMDBContext context, ILogService logService, ILogger<AdminService> logger) : base(context)
        {
            _context = context;
            _jwtService = jwtService;
            _logService = logService;
            _logger = logger;
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var admin = await _context.Admins
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Where(x => x.Account.Application.Name == "CMDB" && x.Account.UserID == model.Username).FirstOrDefaultAsync();
            if (admin == null)
                return null;
            TokenStore.AdminId = admin.AdminId;
            TokenStore.Admin = admin;
            if (string.Equals(admin.Password, new PasswordHasher().EncryptPassword(model.Password)))
            {
                var token = _jwtService.GenerateToken(admin);
                return new AuthenticateResponse()
                {
                    Id = admin.AdminId,
                    UserId = admin.Account.UserID,
                    Level = admin.Level,
                    Token = token
                };
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
                .Where(x => x.AdminId == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Admin?> Create(Admin admin)
        {
            var _admin = await GetById(admin.AdminId);
            if (_admin is null)
                return null;
            admin.LastModfiedAdmin = TokenStore.Admin;
            string pwd = new PasswordHasher().EncryptPassword("cmdb");
            admin.Password = pwd;
            admin.DateSet = DateTime.Now;
            admin.LastModfiedAdmin = TokenStore.Admin;
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            string Value = "Admin with UserID: " + admin.Account.UserID + " and level: " + admin.Level.ToString();
            await _logService.LogCreate(Table, admin.AdminId, Value);
            return admin;
        }
        public async Task<Admin?> Update(Admin admin)
        {
            var _admin = await GetById(admin.AdminId);
            if (_admin is null)
                return null;
            if (admin.Level != _admin.Level)
            {
                _admin.Level = admin.Level;
                _admin.LastModfiedAdmin = TokenStore.Admin;
                await _context.SaveChangesAsync();
                await _logService.LogUpdate(Table, _admin.AdminId, "Level", _admin.Level.ToString(), _admin.Level.ToString());
            }
            return _admin;
        }
        public bool IsExisting(Admin admin)
        {
            bool result = false;
            var admins = _context.Admins.Include(x => x.Account).Where(x => x.Account.UserID == admin.Account.UserID);
            if (admins.Any())
                result = true;
            return result;
        }
        public async Task<bool> HasAdminAccess(HasAdminAccessRequest request)
        {
            var admin = await GetById(request.AdminId);
            if (admin is null) return false;

            var perm = _context.RolePerms
                .Include(x => x.Menu)
                .Include(x => x.Permission)
                .Where(x => x.Level == admin.Level || x.Menu.Label == request.Site || x.Permission.Rights == request.Action).ToList();
            if (perm.Count > 0)
                return true;
            else
                return false;
        }
    }
}
