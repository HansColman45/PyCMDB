using CMDB.API.Models;
using CMDB.Domain.Entities;

namespace CMDB.API.Services
{
    /// <summary>
    /// Account repository interface
    /// </summary>
    public interface IAccountRepository
    {
        AccountDTO Create(AccountDTO account);
        Task<AccountDTO> GetById(int id);
        Task<List<AccountDTO>> GetAll();
        Task<List<AccountDTO>> GetAll(string searchstr);
        Task<AccountDTO> DeActivate(AccountDTO account, string reason);
        Task<AccountDTO> Activate(AccountDTO account);
        Task<bool> IsExisitng(AccountDTO account);
        Task<AccountDTO> Update(AccountDTO account);
        Task AssignAccount2Identity(IdenAccountDTO request);
        Task ReleaseAccountFromIdentity(IdenAccountDTO request);
        Task LogPdfFile(string pdfFile, int id);
        Task<IEnumerable<IdentityAccountInfo>> ListAllFreeAccounts();
    }
}
