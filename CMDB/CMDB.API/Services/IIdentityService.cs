
using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface IIdentityService
    {
        Task<IEnumerable<IdentityDTO>> GetAll();
        Task<IEnumerable<IdentityDTO>> GetAll(string searchStr);
        Task<IdentityDTO?> GetById(int id);
        Task<List<IdentityDTO>> ListAllFreeIdentities();
    }
}
