using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;

namespace CMDB.API.Services
{
    /// <summary>
    /// Admin repository interface
    /// </summary>
    public interface IAdminRepository
    {
        /// <summary>
        /// Will check if the admin exists
        /// </summary>
        /// <param name="admin"></param>
        /// <returns><c>bool</c></returns>
        Task<bool> IsExisting(AdminDTO admin);
        /// <summary>
        /// Will create a new admin
        /// </summary>
        /// <param name="entity"><see cref="AdminDTO"/></param>
        /// <returns><see cref="AdminDTO"/></returns>
        AdminDTO Add(AdminDTO entity);
        /// <summary>
        /// Will update an existing admin
        /// </summary>
        /// <param name="admin"><see cref="AdminDTO"/></param>
        /// <returns><see cref="Admin"/></returns>
        Task<Admin> Update(AdminDTO admin);
        /// <summary>
        /// Will Authenticate the admin to the API
        /// </summary>
        /// <param name="model"><see cref="AuthenticateRequest"/></param>
        /// <returns><see cref="AuthenticateResponse"/></returns>
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        /// <summary>
        /// Will check if the admin has access to the requested part of the site
        /// </summary>
        /// <param name="request"><see cref="HasAdminAccessRequest"/></param>
        /// <returns><c>bool</c></returns>
        Task<bool> HasAdminAccess(HasAdminAccessRequest request);
        /// <summary>
        /// Will get all admins
        /// </summary>
        /// <returns>List of <see cref="AdminDTO"/></returns>
        Task<IEnumerable<AdminDTO>> GetAll();
        /// <summary>
        /// Will get all admins with a search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>List of <see cref="AdminDTO"/></returns>
        Task<IEnumerable<AdminDTO>> GetAll(string searchString);
        /// <summary>
        /// Will get an admin by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AdminDTO"/></returns>
        Task<AdminDTO> GetById(int id);
        /// <summary>
        /// This will activate the admin
        /// </summary>
        /// <param name="admin"><see cref="AdminDTO"/></param>
        /// <returns><see cref="AdminDTO"/></returns>
        Task<AdminDTO> Activate(AdminDTO admin);
        /// <summary>
        /// This will deactivate the admin
        /// </summary>
        /// <param name="admin"><see cref="AdminDTO"/></param>
        /// <param name="reason">The reason of the deactivation</param>
        /// <returns><see cref="AdminDTO"/></returns>
        Task<AdminDTO> DeActivate(AdminDTO admin, string reason);
    }
}
