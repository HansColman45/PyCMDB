using CMDB.API.Models;
using CMDB.Domain.Entities;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Account repository interface
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Creates a new account based on the provided account details.
        /// </summary>
        /// <param name="account">An <see cref="AccountDTO"/> object containing the details of the account to be created. Cannot be null.</param>
        /// <returns>An <see cref="AccountDTO"/> object representing the newly created account</returns>
        AccountDTO Create(AccountDTO account);
        /// <summary>
        /// Retrieves an account by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the account to retrieve. Must be a positive integer.</param>
        /// <returns>The <see cref="AccountDTO"/></returns>
        Task<AccountDTO> GetById(int id);
        /// <summary>
        /// Retrieves a list of all accounts.
        /// </summary>
        /// <returns>Contains a list of <see cref="AccountDTO"/> objects representing all accounts. The list will be empty if no accounts are found.</returns>
        Task<List<AccountDTO>> GetAll();
        /// <summary>
        /// Retrieves a list of all accounts based on a search string.
        /// </summary>
        /// <param name="searchstr"></param>
        /// <returns>Contains a list of <see cref="AccountDTO"/> objects representing all accounts. The list will be empty if no accounts are found.</returns>
        Task<List<AccountDTO>> GetAll(string searchstr);
        /// <summary>
        /// Deactivates the specified account and provides a reason for the deactivation.
        /// </summary>
        /// <remarks>The account will be marked as deactivated, and the provided reason will be recorded
        /// for auditing purposes. Ensure that the account is valid and the reason is meaningful before calling this
        /// method.</remarks>
        /// <param name="account">The <see cref="AccountDTO"/> to be deactivated. Must not be null.</param>
        /// <param name="reason">The reason for deactivating the account. Must not be null or empty.</param>
        /// <returns></returns>
        Task<AccountDTO> DeActivate(AccountDTO account, string reason);
        /// <summary>
        /// Activates the specified account and returns the updated account details.
        /// </summary>
        /// <param name="account">The <see cref="AccountDTO"/> to activate. The account must not be null and should contain valid account information.</param>
        /// <returns></returns>
        Task<AccountDTO> Activate(AccountDTO account);
        /// <summary>
        /// Determines whether the specified account already exists in the system.
        /// </summary>
        /// <param name="account">The <see cref="AccountDTO"/>.</param>
        /// <returns><see langword="true"/> if the account exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> IsExisitng(AccountDTO account);
        /// <summary>
        /// Updates the specified account with new information.
        /// </summary>
        /// <param name="account">The <see cref="AccountDTO"/></param>
        /// <returns></returns>
        Task<AccountDTO> Update(AccountDTO account);
        /// <summary>
        /// Assigns an account to an identity based on the provided request data.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to associate an account with a
        /// specific identity. Ensure that the <paramref name="request"/> object contains all necessary and valid data
        /// before calling this method.</remarks>
        /// <param name="request">The <see cref="IdenAccountDTO"/> details</param>
        /// <returns></returns>
        Task AssignAccount2Identity(IdenAccountDTO request);
        /// <summary>
        /// Releases the association between an account and its identity.
        /// </summary>
        /// <param name="request">An object containing the details of the account and identity to be released. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ReleaseAccountFromIdentity(IdenAccountDTO request);
        /// <summary>
        /// This will log the creation of a PDF file for an account.
        /// </summary>
        /// <param name="pdfFile"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task LogPdfFile(string pdfFile, int id);
        /// <summary>
        /// List all free accounts.
        /// </summary>
        /// <returns>List off <see cref="IdentityAccountInfo"/></returns>
        Task<IEnumerable<IdentityAccountInfo>> ListAllFreeAccounts();
    }
}
