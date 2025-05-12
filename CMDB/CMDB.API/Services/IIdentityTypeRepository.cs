using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface IIdentityTypeRepository
    {
        Task<List<TypeDTO>> GetAll();
        Task<List<TypeDTO>> GetAll(string searchStr);
        Task<TypeDTO> GetById(int id);
        TypeDTO Create(TypeDTO typeDTO);
        Task<TypeDTO> DeActivate(TypeDTO type, string reason);
        Task<TypeDTO> Activate(TypeDTO type);
        Task<TypeDTO> Update(TypeDTO type);
        Task<bool> IsExisitng(TypeDTO type);
    }
}
