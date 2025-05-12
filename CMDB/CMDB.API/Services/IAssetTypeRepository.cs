using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface IAssetTypeRepository
    {
        Task<IEnumerable<AssetTypeDTO>> GetAll();
        Task<IEnumerable<AssetTypeDTO>> GetAll(string searchStr);
        Task<IEnumerable<AssetTypeDTO>> GetByCategory(string category);
        Task<AssetTypeDTO> GetById(int id);
        AssetTypeDTO Create(AssetTypeDTO assetTypeDTO);
        Task<AssetTypeDTO> Update(AssetTypeDTO assetTypeDTO);
        Task<AssetTypeDTO> Deactivate(AssetTypeDTO assetTypeDTO, string reason);
        Task<AssetTypeDTO> Activate(AssetTypeDTO assetTypeDTO);
    }
}
