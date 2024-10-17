using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;

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
}
