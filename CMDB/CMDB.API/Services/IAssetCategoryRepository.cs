using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface IAssetCategoryRepository
    {
        Task<IEnumerable<AssetCategoryDTO>> GetAll();
        Task<IEnumerable<AssetCategoryDTO>> GetAll(string search);
        Task<AssetCategoryDTO> GetById(int id);
        Task<AssetCategoryDTO> GetByCategory(string category);
        Task<bool> IsCategoryExisting(AssetCategoryDTO assetCategory);
        AssetCategoryDTO Create(AssetCategoryDTO assetCategoryDTO);
        Task<AssetCategoryDTO> Update(AssetCategoryDTO assetCategoryDTO);
        Task<AssetCategoryDTO> Delete(AssetCategoryDTO assetCategoryDTO, string reason);
        Task<AssetCategoryDTO> Activate(AssetCategoryDTO assetCategoryDTO);
    }
}
