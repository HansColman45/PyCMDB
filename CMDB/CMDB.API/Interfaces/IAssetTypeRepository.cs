using CMDB.Domain.DTOs;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Repository for AssetType
    /// </summary>
    public interface IAssetTypeRepository
    {
        /// <summary>
        /// This will return all AssetTypes
        /// </summary>
        /// <returns>List of <see cref="AssetTypeDTO"/></returns>
        Task<IEnumerable<AssetTypeDTO>> GetAll();
        /// <summary>
        /// This will return all AssetTypes matching the search string
        /// </summary>
        /// <param name="searchStr"></param>
        /// <returns>List of <see cref="AssetTypeDTO"/></returns>
        Task<IEnumerable<AssetTypeDTO>> GetAll(string searchStr);
        /// <summary>
        /// This will return all AssetTypes matching the category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>List of <see cref="AssetTypeDTO"/></returns>
        Task<IEnumerable<AssetTypeDTO>> GetByCategory(string category);
        /// <summary>
        /// This will return the AssetType matching the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AssetTypeDTO"/></returns>
        Task<AssetTypeDTO> GetById(int id);
        /// <summary>
        /// This will create a new AssetType
        /// </summary>
        /// <param name="assetTypeDTO"><see cref="AssetTypeDTO"/></param>
        /// <returns></returns>
        AssetTypeDTO Create(AssetTypeDTO assetTypeDTO);
        /// <summary>
        /// This will check if the AssetType already exists
        /// </summary>
        /// <param name="assetTypeDTO"><see cref="AssetTypeDTO"/></param>
        /// <returns></returns>
        Task<AssetTypeDTO> Update(AssetTypeDTO assetTypeDTO);
        /// <summary>
        /// This will deactivate the AssetType
        /// </summary>
        /// <param name="assetTypeDTO"><see cref="AssetTypeDTO"/></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<AssetTypeDTO> Deactivate(AssetTypeDTO assetTypeDTO, string reason);
        /// <summary>
        /// This will activate the AssetType
        /// </summary>
        /// <param name="assetTypeDTO"><see cref="AssetTypeDTO"/></param>
        /// <returns></returns>
        Task<AssetTypeDTO> Activate(AssetTypeDTO assetTypeDTO);
    }
}
