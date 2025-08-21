using CMDB.Domain.DTOs;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Defines a contract for managing and retrieving permissions within a system.
    /// </summary>
    /// <remarks>This interface is intended to be implemented by classes that handle permission-related
    /// operations, such as querying, adding, or removing permissions for users, roles, or other entities.</remarks>
    public interface IPermissionRepository
    {
        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing a collection of permissions.</returns>
        Task<IEnumerable<PermissionDTO>> GetAll();
        /// <summary>
        /// Retrieves all permissions that match the specified search criteria.
        /// </summary>
        /// <param name="searchStr">A string used to filter the permissions. If <paramref name="searchStr"/> is null or empty, all permissions
        /// are returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
        /// cref="PermissionDTO"/> objects that match the search criteria.</returns>
        Task<IEnumerable<PermissionDTO>> GetAll(string searchStr);
        /// <summary>
        /// Gets a permission by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the permission.</param>
        /// <returns>A task that represents the asynchronous operation, containing the permission if found; otherwise, null.</returns>
        Task<PermissionDTO> GetById(int id);
        /// <summary>
        /// Adds a new permission.
        /// </summary>
        /// <param name="permission">The permission to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        void Create(PermissionDTO permission);
        /// <summary>
        /// Updates an existing permission.
        /// </summary>
        /// <param name="permission">The permission to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        void Update(PermissionDTO permission);
        /// <summary>
        /// This method will return a list of RolePermissions for a specific Persmission
        /// </summary>
        /// <param name="id">The Id of the permission</param>
        /// <returns>List of <see cref="RolePermissionDTO"/></returns>
        Task<List<RolePermissionDTO>> GetRolePermissionInfo(int id);
    }
}
