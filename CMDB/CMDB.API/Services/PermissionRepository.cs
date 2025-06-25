using CMDB.API.Interfaces;
using CMDB.Domain.DTOs;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// This is the implementation of the <see cref="IPermissionRepository"/> interface.
    /// </summary>
    public class PermissionRepository : GenericRepository, IPermissionRepository
    {
        private static string Table => "permission";
        private PermissionRepository()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public PermissionRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
            
        }
        /// inheritdoc/>
        public void Create(PermissionDTO permission)
        {
            Permission per = new()
            {
                LastModifiedAdminId = TokenStore.AdminId,
                Rights = permission.Right,
                Description = permission.Description
            };
            per.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.CreateLogLine($"permission {permission.Right} with {permission.Description}",TokenStore.Admin.Account.UserID,Table),
                LogDate = DateTime.UtcNow,
            });
            _context.Permissions.Add(per);
        }
        /// inheritdoc/>
        public async Task<IEnumerable<PermissionDTO>> GetAll()
        {
            return await _context.Permissions
                .Select(x => Convert2DTO(x)).ToListAsync();
        }
        /// inheritdoc/>
        public async Task<IEnumerable<PermissionDTO>> GetAll(string searchStr)
        {
            return await _context.Permissions
                .Where(x => EF.Functions.Like(x.Rights, $"%{searchStr}%") || EF.Functions.Like(x.Description, $"%{searchStr}%"))
                .AsNoTracking()
                .Select(x => Convert2DTO(x)).ToListAsync();
        }
        /// inheritdoc/>
        public async Task<PermissionDTO> GetById(int id)
        {
            var perm = await _context.Permissions.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => Convert2DTO(x)).FirstOrDefaultAsync();
            if (perm is not null)
            {
                await GetLogs(perm);
            }
            return perm;
        }
        /// inheritdoc/>
        public void Update(PermissionDTO permission)
        {
            var oldPerm = TrackedPermission(permission.Id);
            if (string.Compare(oldPerm.Rights, permission.Right) != 0)
            {
                string logstring = GenericLogLineCreator.UpdateLogLine("Right", oldPerm.Rights, permission.Right, TokenStore.Admin.Account.UserID, Table);
                oldPerm.Rights = permission.Right;
                oldPerm.Logs.Add(new Log
                {
                    LogText = logstring,
                    LogDate = DateTime.UtcNow,
                });
                
            }
            if(string.Compare(oldPerm.Description, permission.Description) != 0)
            {
                string logstring = GenericLogLineCreator.UpdateLogLine("Description", oldPerm.Description, permission.Description, TokenStore.Admin.Account.UserID, Table);
                oldPerm.Description = permission.Description;
                oldPerm.Logs.Add(new Log
                {
                    LogText = logstring,
                    LogDate = DateTime.UtcNow,
                });
            }
            _context.Permissions.Update(oldPerm);
        }
        /// <summary>
        /// Converts a <see cref="Permission"/> to a <see cref="PermissionDTO"/>.
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static PermissionDTO Convert2DTO(Permission permission)
        {
            return new PermissionDTO
            {
                Id = permission.Id,
                Right = permission.Rights,
                Description = permission.Description,
                LastModifiedAdminId = permission.LastModifiedAdminId
            };
        }
        private async Task GetLogs(PermissionDTO permission)
        {
            permission.Logs = await _context.Logs.AsNoTracking()
                .Include(x => x.RolePerm).Where(x => x.Permission.Id == permission.Id)
                .OrderByDescending(x => x.LogDate)
                .Select(x => Convert2DTO(x)).ToListAsync();
        }
        private Permission TrackedPermission(int id)
        {
            return _context.Permissions.Where(x => x.Id == id).First();
        }
    }
}
