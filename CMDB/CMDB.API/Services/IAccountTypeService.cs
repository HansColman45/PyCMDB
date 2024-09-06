using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface IAccountTypeService
    {
        Task<List<TypeDTO>> GetAll();
        Task<List<TypeDTO>> GetAll(string searchStr);
        Task<TypeDTO> GetById(int id);
        Task<TypeDTO> Create(TypeDTO typeDTO);
        Task<TypeDTO> Update(TypeDTO typeDTO);
        Task<TypeDTO> Delete(TypeDTO typeDTO, string reason);
        Task<TypeDTO> Activate(TypeDTO typeDTO);
    }
}
