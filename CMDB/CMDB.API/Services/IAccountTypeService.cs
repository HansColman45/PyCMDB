using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface IAccountTypeService
    {
        Task<List<AccountTypeDTO>> GetAll();
        Task<List<AccountTypeDTO>> GetAll(string searchStr);
        Task<AccountTypeDTO> GetById(int id);
    }
}
