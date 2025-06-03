using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// The permission repository implementation.
    /// </summary>
    public class RolePermissionRepository: GenericRepository, IRolePermissionRepository
    {
        private RolePermissionRepository()
        {
        }

        private readonly string table = "permission";
        /// <summary>
        /// Constructor for the permission repository.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public RolePermissionRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// inheritdoc/>
        public async Task<List<RolePermissionDTO>> GetAll()
        {
            return await _context.RolePerms
                .Include(x => x.Permission)
                .Include(x => x.Menu)
                .Select(x => Convert2DTO(x))
                .ToListAsync();
        }
        /// inheritdoc/>
        public async Task<List<RolePermissionDTO>> GetAll(string searchStr)
        {
            return await _context.RolePerms
                .Include(x => x.Permission)
                .Include(x => x.Menu)
                .Where(x => EF.Functions.Like(x.Permission.Rights, $"%{searchStr}%") ||
                            EF.Functions.Like(x.Permission.Description, $"%{searchStr}%") ||
                            EF.Functions.Like(x.Menu.Label, $"%{searchStr}%")).AsNoTracking()
                .Select(x => Convert2DTO(x))
                .ToListAsync();
        }
        /// inheritdoc/>
        public async Task<RolePermissionDTO> GetById(int id)
        {
            var rolper = await _context.RolePerms
                .Include(x => x.Permission)
                .Include(x => x.Menu)
                .Where(x => x.Id == id).AsNoTracking()
                .Select(x => Convert2DTO(x))
                .FirstOrDefaultAsync();
            if(rolper is not null)
            {
                await GetLogs(rolper);
            }
            return rolper;
        }
        /// inheritdoc/>
        public RolePermissionDTO Create(RolePermissionDTO permission)
        {
            RolePerm rolePerm = new()
            {
                Level = permission.Level,
                LastModifiedAdminId = TokenStore.AdminId,
                PermissionId = permission.Permission.Id,
                MenuId = permission.Menu.MenuId,
            };
            string value = $"permission {permission.Permission.Right} has been granted for level {permission.Level} for menu {permission.Menu.Label}";
            rolePerm.Logs.Add(new Log()
            {
                LogText = GenericLogLineCreator.CreateLogLine(value, TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.Now,
            });
            _context.RolePerms.Add(rolePerm);
            return permission;
        }
        /// inheritdoc/>
        public Task<Permission> Delete(RolePermissionDTO permission)
        {
            throw new NotImplementedException();
        }
        /// inheritdoc/>
        public Task<Permission> Update(RolePermissionDTO permission)
        {
            throw new NotImplementedException();
        }
        /// inheritdoc/>
        public Task<bool> IsExisitng(RolePermissionDTO permission)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// This function will convert the RolePerm entity to a RolePermissionDTO.
        /// </summary>
        /// <param name="rolePerm"><see cref="RolePerm"/></param>
        /// <returns><see cref="RolePermissionDTO"/></returns>
        public static RolePermissionDTO Convert2DTO(RolePerm rolePerm)
        {
            return new RolePermissionDTO
            {
                Id = rolePerm.Id,
                Level = rolePerm.Level,
                LastModifiedAdminId = rolePerm.LastModifiedAdminId,
                Permission = new PermissionDTO
                {
                    Id = rolePerm.Permission.Id,
                    Right = rolePerm.Permission.Rights,
                    Description = rolePerm.Permission.Description,
                    LastModifiedAdminId = rolePerm.Permission.LastModifiedAdminId
                },
                Menu = new()
                {
                    MenuId = rolePerm.Menu.MenuId,
                    Label = rolePerm.Menu.Label
                }
            };
        }
        private async Task GetLogs(RolePermissionDTO permission)
        {
            permission.Logs = await _context.Logs.AsNoTracking()
                .Include(x => x.RolePerm).Where(x => x.Permission.Id == permission.Id)
                .OrderByDescending(x => x.LogDate)
                .Select(x => Convert2DTO(x)).ToListAsync();
        }
    }
}
