using CMDB.Domain.Entities;

namespace CMDB.API.Services
{
    public interface IIdentityService
    {
        Task<IEnumerable<Identity>> GetAll();
        Task<Identity?> GetById(int id);
        Task<Account?> GetAccountById(int id);
    }
}
