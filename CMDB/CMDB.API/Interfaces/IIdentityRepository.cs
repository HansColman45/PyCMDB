using CMDB.API.Models;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Repository interface for managing Identity entities and their related assignments.
    /// Provides methods for CRUD operations, device/account/mobile/subscription assignments, and status management.
    /// </summary>
    public interface IIdentityRepository
    {
        /// <summary>
        /// Gets an Identity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the Identity.</param>
        /// <returns>The <see cref="IdentityDTO"/> if found; otherwise, null.</returns>
        Task<IdentityDTO> GetById(int id);

        /// <summary>
        /// Gets all Identities.
        /// </summary>
        /// <returns>A collection of all <see cref="IdentityDTO"/>.</returns>
        Task<IEnumerable<IdentityDTO>> GetAll();

        /// <summary>
        /// Gets all Identities matching the specified search string.
        /// </summary>
        /// <param name="searchStr">The search string to filter Identities.</param>
        /// <returns>A collection of matching <see cref="IdentityDTO"/>.</returns>
        Task<IEnumerable<IdentityDTO>> GetAll(string searchStr);

        /// <summary>
        /// Lists all free (unassigned) Identities for a given site part.
        /// </summary>
        /// <param name="sitePart">The site part to filter Identities.</param>
        /// <returns>A collection of free <see cref="IdentityDTO"/>.</returns>
        Task<IEnumerable<IdentityDTO>> ListAllFreeIdentities(string sitePart);

        /// <summary>
        /// Creates a new Identity.
        /// </summary>
        /// <param name="identityDTO">The <see cref="IdentityDTO"/> to create.</param>
        /// <returns>The created <see cref="IdentityDTO"/></returns>
        IdentityDTO Create(IdentityDTO identityDTO);

        /// <summary>
        /// Logs a PDF file for the specified Identity.
        /// </summary>
        /// <param name="pdfFile">The PDF file name or path.</param>
        /// <param name="id">The Identity's unique identifier.</param>
        Task LogPdfFile(string pdfFile, int id);

        /// <summary>
        /// Deactivates the specified Identity with a reason.
        /// </summary>
        /// <param name="identity">The <see cref="IdentityDTO"/> to deactivate.</param>
        /// <param name="reason">The reason for deactivation.</param>
        /// <returns>The updated <see cref="IdentityDTO"/>.</returns>
        Task<IdentityDTO> Deactivate(IdentityDTO identity, string reason);

        /// <summary>
        /// Activates the specified Identity.
        /// </summary>
        /// <param name="identity">The <see cref="IdentityDTO"/> to activate.</param>
        /// <returns>The updated <see cref="IdentityDTO"/>.</returns>
        Task<IdentityDTO> Activate(IdentityDTO identity);

        /// <summary>
        /// Updates the specified Identity.
        /// </summary>
        /// <param name="identity">The <see cref="IdentityDTO"/> to update.</param>
        /// <returns>The updated <see cref="IdentityDTO"/>.</returns>
        Task<IdentityDTO> Update(IdentityDTO identity);

        /// <summary>
        /// Checks if the specified Identity already exists.
        /// </summary>
        /// <param name="identity">The IdentityDTO to check.</param>
        /// <returns>True if the Identity exists; otherwise, false.</returns>
        Task<bool> IsExisting(IdentityDTO identity);

        /// <summary>
        /// Assigns devices to the specified Identity.
        /// </summary>
        /// <param name="identity">The <see cref="IdentityDTO"/> to assign devices to.</param>
        /// <param name="assetTags">A list of device asset tags to assign.</param>
        Task AssignDevices(IdentityDTO identity, List<string> assetTags);

        /// <summary>
        /// Releases devices from the specified Identity.
        /// </summary>
        /// <param name="identity">The <see cref="IdentityDTO"/> to release devices from.</param>
        /// <param name="assetTags">A list of device asset tags to release.</param>
        Task ReleaseDevices(IdentityDTO identity, List<string> assetTags);

        /// <summary>
        /// Assigns mobiles to the specified Identity.
        /// </summary>
        /// <param name="identity">The <see cref="IdentityDTO"/> to assign mobiles to.</param>
        /// <param name="mobileIDs">A list of mobile IDs to assign.</param>
        Task AssignMobile(IdentityDTO identity, List<int> mobileIDs);

        /// <summary>
        /// Releases mobiles from the specified Identity.
        /// </summary>
        /// <param name="identity">The <see cref="IdentityDTO"/> to release mobiles from.</param>
        /// <param name="mobileIDs">A list of mobile IDs to release.</param>
        Task ReleaseMobile(IdentityDTO identity, List<int> mobileIDs);

        /// <summary>
        /// Assigns an account to an Identity.
        /// </summary>
        /// <param name="idenAccount">The <see cref="IdenAccountDTO"/> representing the assignment.</param>
        Task AssignAccount(IdenAccountDTO idenAccount);

        /// <summary>
        /// Releases an account from an Identity.
        /// </summary>
        /// <param name="idenAccount">The <see cref="IdenAccountDTO"/> representing the release.</param>
        Task ReleaseAccount(IdenAccountDTO idenAccount);

        /// <summary>
        /// Assigns subscriptions to the specified Identity.
        /// </summary>
        /// <param name="identity">The <see cref="IdentityDTO"/> to assign subscriptions to.</param>
        /// <param name="subscriptionIds">A list of subscription IDs to assign.</param>
        Task AssignSubscription(IdentityDTO identity, List<int> subscriptionIds);

        /// <summary>
        /// Releases subscriptions from the specified Identity.
        /// </summary>
        /// <param name="identity">The <see cref="IdentityDTO"/> to release subscriptions from.</param>
        /// <param name="subscriptionIds">A list of subscription IDs to release.</param>
        Task ReleaseSubscription(IdentityDTO identity, List<int> subscriptionIds);
    }
}
