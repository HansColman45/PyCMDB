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
        Task<AccountDTO?> DeactivateById(int id);
        Task<AccountDTO?> ActivateById(int id);
        bool IsAccountExisting(AccountDTO account, string UserID = "", int application = 0);
    }
}
