using CMDB.API.Models;
using CMDB.Domain.Entities;

namespace CMDB.API.Services
{
    /// <summary>
    /// Repository interface for <see cref="RolePerm"/>.
    /// </summary>
    public interface IRolePermissionRepository
    {
        /// <summary>
        /// This will list all Permissions.
        /// </summary>
        /// <returns>List of <see cref="RolePermissionDTO"/></returns>
        Task<List<RolePermissionDTO>> GetAll();
        /// <summary>
        /// This will list all Permissions matching the search string.
        /// </summary>
        /// <param name="searchStr"></param>
        /// <returns>List of <see cref="RolePermissionDTO"/></returns>
        Task<List<RolePermissionDTO>> GetAll(string searchStr);
        /// <summary>
        /// This will return a Permission by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="RolePermissionDTO"/></returns>
        Task<RolePermissionDTO> GetById(int id);
        /// <summary>
        /// This will create a new Permission.
        /// </summary>
        /// <param name="permission"><see cref="RolePermissionDTO"/></param>
        /// <returns></returns>
        RolePermissionDTO Create(RolePermissionDTO permission);
        /// <summary>
        /// This will delete a Permission.
        /// </summary>
        /// <param name="permission"><see cref="Permission"/></param>
        /// <returns></returns>
        Task<Permission> Delete(RolePermissionDTO permission);
        /// <summary>
        /// This will update a Permission.
        /// </summary>
        /// <param name="permission"><see cref="RolePermissionDTO"/></param>
        /// <returns></returns>
        Task<Permission> Update(RolePermissionDTO permission);
        /// <summary>
        /// This will check if a Permission already exists.
        /// </summary>
        /// <param name="permission"><see cref="RolePermissionDTO"/></param>
        /// <returns></returns>
        Task<bool> IsExisitng(RolePermissionDTO permission);
    }
}
