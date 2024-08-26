using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Domain.Responses;

namespace CMDB.API.Services
{
    public interface IAdminService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<AdminDetailResponce>> GetAll();
        Task<Admin?> GetById(int id);
        Task<Admin?> Create(Admin admin);
        Task<Admin?> Update(Admin admin);
        Task<bool> HasAdminAccess(HasAdminAccessRequest request);
        bool IsExisting(Admin admin);
    }
}
