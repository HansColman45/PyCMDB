using CMDB.API.Models;

namespace CMDB.API.Services
{
    /// <summary>
    /// Repository interface for IdentityType
    /// </summary>
    public interface IIdentityTypeRepository
    {
        /// <summary>
        /// This will list all IdentityTypes
        /// </summary>
        /// <returns>List of <see cref="TypeDTO"/></returns>
        Task<List<TypeDTO>> GetAll();
        /// <summary>
        /// This will list all IdentityTypes matching the search string
        /// </summary>
        /// <param name="searchStr"></param>
        /// <returns>List of <see cref="TypeDTO"/></returns>
        Task<List<TypeDTO>> GetAll(string searchStr);
        /// <summary>
        /// This will return a IdentityType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="TypeDTO"/></returns>
        Task<TypeDTO> GetById(int id);
        /// <summary>
        /// This will create a new IdentityType
        /// </summary>
        /// <param name="typeDTO"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        TypeDTO Create(TypeDTO typeDTO);
        /// <summary>
        /// This will delete a IdentityType
        /// </summary>
        /// <param name="type"><see cref="TypeDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<TypeDTO> DeActivate(TypeDTO type, string reason);
        /// <summary>
        /// This will activate a IdentityType
        /// </summary>
        /// <param name="type"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        Task<TypeDTO> Activate(TypeDTO type);
        /// <summary>
        /// This will update a IdentityType
        /// </summary>
        /// <param name="type"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        Task<TypeDTO> Update(TypeDTO type);
        /// <summary>
        /// This will check if a IdentityType already exists
        /// </summary>
        /// <param name="type"><see cref="TypeDTO"/></param>
        /// <returns></returns>
        Task<bool> IsExisitng(TypeDTO type);
    }
}
