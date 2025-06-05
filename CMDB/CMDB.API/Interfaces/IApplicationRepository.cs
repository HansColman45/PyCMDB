using CMDB.API.Models;
using CMDB.Domain.Entities;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Interface for Application repository.
    /// Provides methods to manage Application entities, including retrieval, update, activation, and deactivation.
    /// </summary>
    public interface IApplicationRepository
    {
        /// <summary>
        /// Retrieves all applications.
        /// </summary>
        /// <returns>A collection of <see cref="ApplicationDTO"/>.</returns>
        Task<IEnumerable<ApplicationDTO>> GetAll();

        /// <summary>
        /// Retrieves all applications that match the specified search string.
        /// </summary>
        /// <param name="searchStr">The search string to filter applications.</param>
        /// <returns>A collection of <see cref="ApplicationDTO"/> that match the search criteria.</returns>
        Task<IEnumerable<ApplicationDTO>> GetAll(string searchStr);

        /// <summary>
        /// Retrieves an application by its unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the application.</param>
        /// <returns>The <see cref="ApplicationDTO"/> if found; otherwise, null.</returns>
        Task<ApplicationDTO> GetById(int Id);

        /// <summary>
        /// Updates the specified application.
        /// </summary>
        /// <param name="appDTO">The <see cref="ApplicationDTO"/></param>
        /// <returns>The updated Application entity.</returns>
        Task<Application> Update(ApplicationDTO appDTO);

        /// <summary>
        /// Deactivates the specified application with a given reason.
        /// </summary>
        /// <param name="application">The <see cref="ApplicationDTO"/>.</param>
        /// <param name="reason">The reason for deactivation.</param>
        /// <returns>The deactivated Application entity.</returns>
        Task<Application> DeActivate(ApplicationDTO application, string reason);

        /// <summary>
        /// Activates the specified application.
        /// </summary>
        /// <param name="application">The <see cref="ApplicationDTO"/>.</param>
        /// <returns>The activated Application entity.</returns>
        Task<Application> Activate(ApplicationDTO application);
    }
}
