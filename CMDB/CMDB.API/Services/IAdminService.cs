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
        Task Create(Admin admin);
        Task Update(Admin admin);
    }
}
