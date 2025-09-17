using CMDB.API.Interfaces;
using CMDB.Domain.DTOs;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// Admin repository
    /// </summary>
    public class AdminRepository : GenericRepository, IAdminRepository
    {
        private readonly JwtService _jwtService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="jwtService"></param>
        public AdminRepository(CMDBContext context, ILogger logger, JwtService jwtService) : base(context, logger)
        {
            _jwtService = jwtService;
        }
        ///inheritdoc />
        public async Task<bool> IsExisting(AdminDTO admindto)
        {
            bool result = false;
            var admin = await GetAdminByID(admindto.AdminId);
            var account = await _context.Accounts.Where(x => x.AccID == admin.AccountId).AsNoTracking().FirstOrDefaultAsync();
            if (string.Compare(admindto.Account.UserID, account.UserID) == 0 && admindto.Account.ApplicationId == account.ApplicationId)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        ///inheritdoc />
        public AdminDTO Add(AdminDTO entity)
        {
            Admin admin = new()
            {
                AccountId = entity.Account.AccID,
                Level = entity.Level,
                active = 1,
                DateSet = DateTime.UtcNow,
                LastModifiedAdminId = TokenStore.AdminId
            };
            admin.Password = new PasswordHasher().EncryptPassword("Br!ght1CmDb");
            string value = "Admin with UserID: " + entity.Account.UserID + " and level: " + entity.Level.ToString();
            var logLine = GenericLogLineCreator.CreateLogLine(value, TokenStore.Admin.Account.UserID, "admin");
            admin.Logs.Add(new()
            {
                LogText = logLine,
                LogDate = DateTime.UtcNow,
            });
            _context.Admins.Add(admin);
            return entity;
        }
        ///inheritdoc />
        public async Task<AdminDTO> Update(AdminDTO admin)
        {
            var _admin = await GetAdminByID(admin.AdminId);
            if (admin.Level != _admin.Level)
            {
                var logLine = GenericLogLineCreator.UpdateLogLine("Level", $"{_admin.Level}", $"{admin.Level}", TokenStore.Admin.Account.UserID, "admin");
                _admin.Level = admin.Level;
                _admin.LastModifiedAdmin = TokenStore.Admin;
                _admin.Logs.Add(new()
                {
                    LogText = logLine,
                    LogDate = DateTime.UtcNow
                });
                _context.Admins.Update(_admin);
            }
            return admin;
        }
        ///inheritdoc />
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var admin = await _context.Admins
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Where(x => x.Account.Application.Name == "CMDB" && x.Account.UserID == model.Username).AsNoTracking()
                .FirstOrDefaultAsync();
            if (admin == null)
                return null;
            var idenAccounts = await _context.IdenAccounts
                .Where(x => x.AccountId == admin.Account.AccID).AsNoTracking().ToListAsync();
            bool validAccount = false;
            foreach (var idenacc in idenAccounts)
            {
                _logger.LogInformation($"Found IdenAccount with Id: {idenacc.ID} and valid from: {idenacc.ValidFrom} unti: {idenacc.ValidUntil} " +
                    $"for Identity:{idenacc.IdentityId}");
                if (idenacc.ValidUntil.ToUniversalTime() >= DateTime.UtcNow && idenacc.ValidFrom.ToUniversalTime() < DateTime.UtcNow)
                {
                    _logger.LogInformation("Valid Account");
                    validAccount = true;
                }
            }
            if (!validAccount) {
                throw new Exception("Not a valid account");
            }
            TokenStore.AdminId = admin.AdminId;
            TokenStore.Admin = admin;
            PasswordHasher passwordHasher = new();
            if (!string.Equals(admin.Password, passwordHasher.EncryptPassword(model.Password)))
                return null;

            var token = _jwtService.GenerateToken(admin);
            return new AuthenticateResponse()
            {
                Id = admin.AdminId,
                UserId = admin.Account.UserID,
                Level = admin.Level,
                Token = token
            };
        }
        ///inheritdoc />
        public async Task<AuthenticateResponse> LogOut(AuthenticateRequest model)
        {
            var admin = await _context.Admins
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Where(x => x.Account.Application.Name == "CMDB" && x.Account.UserID == model.Username).AsNoTracking()
                .FirstOrDefaultAsync();
            if (admin == null)
                return null;
            TokenStore.AdminId = admin.AdminId;
            TokenStore.Admin = admin;
            return new AuthenticateResponse()
            {
                Id = admin.AdminId,
                UserId = admin.Account.UserID,
                Level = admin.Level,
                Token = null
            };
        }
        /// inheritdoc />
        public async Task<bool> HasAdminAccess(HasAdminAccessRequest request)
        {
            _logger.LogTrace($"Checking access for admin ID: {request.AdminId} to site: {request.Site} with permission: {request.Permission}");
            var admin = await _context.Admins.Where(x => x.AdminId == request.AdminId).AsNoTracking().FirstOrDefaultAsync();
            if (admin is null) return false;
            var permission = request.Permission;
            var perm = _context.RolePerms
                .Include(x => x.Menu)
                .Include(x => x.Permission)
                .Where(x => x.Level == admin.Level && x.Menu.Label == request.Site && x.Permission.Id == (int)permission).AsNoTracking()
                .ToList();
            if (perm.Count > 0)
                return true;
            else
                return false;
        }
        ///inheritdoc />
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
        ///inheritdoc />
        public async Task<AdminDTO> GetById(int id)
        {
             var admin = await _context.Admins
                .Include(x => x.Account)
                .ThenInclude(x => x.Type)
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Where(x => x.AdminId == id).AsNoTracking()
                .Select(x => ConvertAdmin(x))
                .FirstOrDefaultAsync();
            if (admin is not null)
            {
                GetLogs("admin", admin.AdminId, admin);
            }
            return admin;
        }
        ///inheritdoc />
        public async Task<IEnumerable<AdminDTO>> GetAll(string searchString)
        {
            string searhterm = "%" + searchString + "%";
            return await _context.Admins
                .Include(x => x.Account)
                .ThenInclude(x => x.Type)
                .Include(x => x.Account)
                .ThenInclude(x => x.Application)
                .Where(x => EF.Functions.Like(x.Level.ToString(), searhterm) || EF.Functions.Like(x.Account.UserID, searhterm)).AsNoTracking()
                .Select(x => ConvertAdmin(x))
                .ToListAsync();
        }
        /// <summary>
        /// Converts the admin to a DTO
        /// </summary>
        /// <param name="admin"></param>
        /// <returns><see cref="AdminDTO"/></returns>
        public static AdminDTO ConvertAdmin(Admin admin)
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
        ///inheritdoc />
        public async Task<AdminDTO> Activate(AdminDTO admin)
        {
            var _admin = await GetAdminByID(admin.AdminId);
            _admin.active = 1;
            _admin.DeactivateReason = null;
            _admin.LastModifiedAdmin = TokenStore.Admin;
            var logLine = GenericLogLineCreator.ActivateLogLine($"Admin with UserID: {admin.Account.UserID}", TokenStore.Admin.Account.UserID, "admin");
            _admin.Logs.Add(new()
            {
                LogText = logLine,
                LogDate = DateTime.UtcNow
            });
            _context.Admins.Update(_admin);
            return admin;
        }
        ///inheritdoc />
        public async Task<AdminDTO> DeActivate(AdminDTO admin, string reason)
        {
            var _admin = await GetAdminByID(admin.AdminId);
            _admin.active = 0;
            _admin.DeactivateReason = reason;
            _admin.LastModifiedAdmin = TokenStore.Admin;
            var logLine = GenericLogLineCreator.DeleteLogLine($"Admin with UserID: {admin.Account.UserID}", TokenStore.Admin.Account.UserID,reason, "admin");
            _admin.Logs.Add(new()
            {
                LogText = logLine,
                LogDate = DateTime.UtcNow
            });
            _context.Admins.Update(_admin);
            return admin;
        }

        private async Task<Admin> GetAdminByID(int Id)
        {
            return await _context.Admins.Where(x => x.AdminId == Id).FirstOrDefaultAsync();
        }
    }
}
