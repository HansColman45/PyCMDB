using CMDB.API.Models;
using CMDB.Domain.Entities;

namespace CMDB.API.Services
{
    public interface IApplicationRepository
    {
        Task<IEnumerable<ApplicationDTO>> GetAll();
        Task<IEnumerable<ApplicationDTO>> GetAll(string searchStr);
        Task<ApplicationDTO> GetById(int Id);
        Task<Application> Update(ApplicationDTO appDTO);
        Task<Application> DeActivate(ApplicationDTO application, string reason);
        Task<Application> Activate(ApplicationDTO application);
    }
}
