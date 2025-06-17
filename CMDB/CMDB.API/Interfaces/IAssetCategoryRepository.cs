using CMDB.Domain.DTOs;

namespace CMDB.API.Interfaces
{
    /// <summary>  
    /// Interface for managing asset categories in the system.  
    /// </summary>  
    public interface IAssetCategoryRepository
    {
        /// <summary>  
        /// Retrieves all asset categories.  
        /// </summary>  
        /// <returns>A collection of all <see cref="AssetCategoryDTO"/>.</returns>  
        Task<IEnumerable<AssetCategoryDTO>> GetAll();

        /// <summary>  
        /// Retrieves all asset categories that match the specified search criteria.  
        /// </summary>  
        /// <param name="search">The search term to filter asset categories.</param>  
        /// <returns>A collection of matching <see cref="AssetCategoryDTO"/></returns>  
        Task<IEnumerable<AssetCategoryDTO>> GetAll(string search);

        /// <summary>  
        /// Retrieves an asset category by its unique identifier.  
        /// </summary>  
        /// <param name="id">The unique identifier of the asset category.</param>  
        /// <returns>The <see cref="AssetCategoryDTO"/></returns>  
        Task<AssetCategoryDTO> GetById(int id);

        /// <summary>  
        /// Retrieves an asset category by its category name.  
        /// </summary>  
        /// <param name="category">The name of the category.</param>  
        /// <returns>The <see cref="AssetCategoryDTO"/>.</returns>  
        Task<AssetCategoryDTO> GetByCategory(string category);

        /// <summary>  
        /// Checks if a given asset category already exists.  
        /// </summary>  
        /// <param name="assetCategory">The asset category to check.</param>  
        /// <returns>True if the category exists, otherwise false.</returns>  
        Task<bool> IsCategoryExisting(AssetCategoryDTO assetCategory);

        /// <summary>  
        /// Creates a new asset category.  
        /// </summary>  
        /// <param name="assetCategoryDTO">The <see cref="AssetCategoryDTO"/> to create.</param>  
        /// <returns><see cref="AssetCategoryDTO"/></returns>  
        AssetCategoryDTO Create(AssetCategoryDTO assetCategoryDTO);

        /// <summary>  
        /// Updates an existing asset category.  
        /// </summary>  
        /// <param name="assetCategoryDTO">The <see cref="AssetCategoryDTO"/> to update.</param>  
        /// <returns>The updated <see cref="AssetCategoryDTO"/></returns>  
        Task<AssetCategoryDTO> Update(AssetCategoryDTO assetCategoryDTO);

        /// <summary>  
        /// Deletes an asset category with a specified reason.  
        /// </summary>  
        /// <param name="assetCategoryDTO">The asset category to delete.</param>  
        /// <param name="reason">The reason for deletion.</param>  
        /// <returns>The deleted asset category.</returns>  
        Task<AssetCategoryDTO> Delete(AssetCategoryDTO assetCategoryDTO, string reason);

        /// <summary>  
        /// Activates a previously deactivated asset category.  
        /// </summary>  
        /// <param name="assetCategoryDTO">The asset category to activate.</param>  
        /// <returns>The activated asset category.</returns>  
        Task<AssetCategoryDTO> Activate(AssetCategoryDTO assetCategoryDTO);
    }
}
