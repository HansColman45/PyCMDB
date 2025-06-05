using CMDB.API.Interfaces;
using CMDB.API.Models;
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
        public Task Create(PermissionDTO permission)
        {
            throw new NotImplementedException();
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
        public Task Update(PermissionDTO permission)
        {
            throw new NotImplementedException();
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
    }
}
