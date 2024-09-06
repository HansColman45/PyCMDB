using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Domain.Requests;

namespace CMDB.API.Services
{
    public interface IAccountService
    {
        Task<List<AccountDTO>> ListAll();
        Task<List<AccountDTO>> ListAll(string searchString);
        Task<AccountDTO?> GetById(int id);
        Task<AccountDTO?> CreateNew(AccountDTO account);
        Task<AccountDTO?> Update(AccountDTO account);
        Task<AccountDTO?> Deactivate(AccountDTO account, string reason);
        Task<AccountDTO?> Activate(AccountDTO account);
        Task<bool> IsAccountExisting(AccountDTO account);
        Task<AccountDTO> AssignIdentity(IdenAccountDTO request);
    }
}
