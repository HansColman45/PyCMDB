using CMDB.API.Models;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Repository for managing <see cref="TypeDTO"/> entities related to account types.
    /// </summary>
    public interface IAccountTypeRepository
    {
        /// <summary>
        /// Retrieves all <see cref="TypeDTO"/> entities.
        /// </summary>
        /// <returns>A list of all <see cref="TypeDTO"/> objects.</returns>
        Task<List<TypeDTO>> GetAll();

        /// <summary>
        /// Retrieves all <see cref="TypeDTO"/> entities that match the specified search string.
        /// </summary>
        /// <param name="searchStr">The search string to filter account types.</param>
        /// <returns>A list of matching <see cref="TypeDTO"/> objects.</returns>
        Task<List<TypeDTO>> GetAll(string searchStr);

        /// <summary>
        /// Retrieves a <see cref="TypeDTO"/> entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the account type.</param>
        /// <returns>The matching <see cref="TypeDTO"/> object, or null if not found.</returns>
        Task<TypeDTO> GetById(int id);

        /// <summary>
        /// Creates a new <see cref="TypeDTO"/> entity.
        /// </summary>
        /// <param name="typeDTO">The <see cref="TypeDTO"/> object to create.</param>
        /// <returns>The created <see cref="TypeDTO"/> object.</returns>
        TypeDTO Create(TypeDTO typeDTO);

        /// <summary>
        /// Deactivates a <see cref="TypeDTO"/> entity with a specified reason.
        /// </summary>
        /// <param name="type">The <see cref="TypeDTO"/> object to deactivate.</param>
        /// <param name="reason">The reason for deactivation.</param>
        /// <returns>The updated <see cref="TypeDTO"/> object.</returns>
        Task<TypeDTO> DeActivate(TypeDTO type, string reason);

        /// <summary>
        /// Activates a previously deactivated <see cref="TypeDTO"/> entity.
        /// </summary>
        /// <param name="type">The <see cref="TypeDTO"/> object to activate.</param>
        /// <returns>The updated <see cref="TypeDTO"/> object.</returns>
        Task<TypeDTO> Activate(TypeDTO type);

        /// <summary>
        /// Updates an existing <see cref="TypeDTO"/> entity.
        /// </summary>
        /// <param name="type">The <see cref="TypeDTO"/> object to update.</param>
        /// <returns>The updated <see cref="TypeDTO"/> object.</returns>
        Task<TypeDTO> Update(TypeDTO type);

        /// <summary>
        /// Checks if a <see cref="TypeDTO"/> entity already exists.
        /// </summary>
        /// <param name="type">The <see cref="TypeDTO"/> object to check.</param>
        /// <returns>True if the entity exists; otherwise, false.</returns>
        Task<bool> IsExisitng(TypeDTO type);
    }
}
