using CMDB.API.Models;

namespace CMDB.API.Services
{
    public interface IApplicationService
    {
        Task<List<ApplicationDTO>> GetAll();
        Task<List<ApplicationDTO>> GetAll(string searchStr);
        Task<ApplicationDTO> GetById(int id);
    }
}
