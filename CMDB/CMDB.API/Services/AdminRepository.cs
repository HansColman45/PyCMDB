using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface IAdminRepository
    {
        bool IsExisting(Admin admin);
        Admin Add(Admin entity);
        Task<Admin> Update(Admin admin, int adminId);
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<bool> HasAdminAccess(HasAdminAccessRequest request);
        Task<IEnumerable<AdminDTO>> GetAll();
        Task<IEnumerable<AdminDTO>> GetAll(string searchString);
        Task<AdminDTO?> GetById(int id);
    }
    public class AdminRepository : GenericRepository, IAdminRepository
    {
        private readonly JwtService _jwtService;
        public AdminRepository(CMDBContext context, ILogger logger, JwtService jwtService) : base(context, logger)
        {
            _jwtService = jwtService;
        }
        public bool IsExisting(Admin admin)
        {
            bool result = false;
            var admins = _context.Admins.AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Application).AsNoTracking();
            if (admins.Any())
                result = true;
            return result;
        }
        public Admin Add(Admin entity)
        {
            string value = "Admin with UserID: " + entity.Account.UserID + " and level: " + entity.Level.ToString();
            var logLine = GenericLogLineCreator.CreateLogLine(value, TokenStore.Admin.Account.UserID, "admin");
            entity.Logs.Add(new()
            {
                LogText = logLine,
                LogDate = DateTime.UtcNow,
                AdminId = entity.AdminId
            });
            _context.Admins.Add(entity);
            return entity;
        }
        public async Task<Admin> Update(Admin admin, int adminId)
        {
            var _admin = await GetAdminByID(adminId);
            if (admin.Level != _admin.Level)
            {
                _admin.Level = admin.Level;
                _admin.LastModfiedAdmin = TokenStore.Admin;
                var logLine = GenericLogLineCreator.UpdateLogLine("Level", $"{_admin.Level}", $"{admin.Level}", TokenStore.Admin.Account.UserID, "admin");
                _admin.Logs.Add(new()
                {
                    LogText = logLine,
                    LogDate = DateTime.UtcNow
                });
                _context.Admins.Update(admin);
                return _admin;
            }
            return _admin;
        }
        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var admin = await _context.Admins.AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Application).AsNoTracking()
                .Where(x => x.Account.Application.Name == "CMDB" && x.Account.UserID == model.Username).AsNoTracking()
                .FirstOrDefaultAsync();
            if (admin == null)
                return null;
            TokenStore.AdminId = admin.AdminId;
            TokenStore.Admin = admin;
            if (string.Equals(admin.Password, new PasswordHasher().EncryptPassword(model.Password)))
            {
                var token = _jwtService.GenerateToken(_context, admin);
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
        public async Task<bool> HasAdminAccess(HasAdminAccessRequest request)
        {
            var admin = await GetAdminByID(request.AdminId);
            if (admin is null) return false;

            var perm = _context.RolePerms.AsNoTracking()
                .Include(x => x.Menu).AsNoTracking()
                .Include(x => x.Permission).AsNoTracking()
                .Where(x => x.Level == admin.Level || x.Menu.Label == request.Site || x.Permission.Rights == request.Action).AsNoTracking()
                .ToList();
            if (perm.Count > 0)
                return true;
            else
                return false;
        }
        public async Task<IEnumerable<AdminDTO>> GetAll()
        {
            return await _context.Admins.AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Application).AsNoTracking()
                .Select(x => ConvertAdmin(x))
                .ToListAsync();
        }
        public async Task<AdminDTO?> GetById(int id)
        {
            return await _context.Admins.AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Application).AsNoTracking()
                .Where(x => x.AdminId == id)
                .Select(x => ConvertAdmin(x))
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<AdminDTO>> GetAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            return await _context.Admins.AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Type).AsNoTracking()
                .Include(x => x.Account)
                .ThenInclude(x => x.Application).AsNoTracking()
                .Where(x => EF.Functions.Like(x.Level.ToString(), searhterm) || EF.Functions.Like(x.Account.UserID, searhterm)).AsNoTracking()
                .Select(x => ConvertAdmin(x))
                .ToListAsync();
        }
        private async Task<Admin?> GetAdminByID(int Id)
        {
            return await _context.Admins.Where(x => x.AdminId == Id).FirstOrDefaultAsync();
        }
        public static AdminDTO ConvertAdmin(in Admin admin)
        {
            return new()
            {
                AccountId = admin.AccountId,
                Active = admin.active,
                DateSet = admin.DateSet,
                DeactivateReason = admin.DeactivateReason,
                LastModifiedAdminId = admin.LastModifiedAdminId,
                Level = admin.Level,
                AdminId = admin.AdminId,
                Account = new()
                {
                    AccID = admin.Account.AccID,
                    Active = admin.Account.active,
                    ApplicationId = admin.Account.ApplicationId,
                    DeactivateReason = admin.Account.DeactivateReason,
                    LastModifiedAdminId = admin.Account.LastModifiedAdminId,
                    TypeId = admin.Account.TypeId,
                    UserID = admin.Account.UserID,
                    Type = new()
                    {
                        TypeId = admin.Account.Type.TypeId,
                        Type = admin.Account.Type.Type,
                        Description = admin.Account.Type.Description,
                        Active = admin.Account.Type.active,
                        DeactivateReason = admin.Account.Type.DeactivateReason,
                        LastModifiedAdminId= admin.Account.LastModifiedAdminId
                    },
                    Application = new()
                    {
                        AppID = admin.Account.Application.AppID,
                        Active = admin.Account.Application.active,
                        DeactivateReason= admin.Account.Application.DeactivateReason,
                        LastModifiedAdminId = admin.Account.Application.LastModifiedAdminId,
                        Name = admin.Account.Application.Name
                    }
                }
            };
        }
    }
}
