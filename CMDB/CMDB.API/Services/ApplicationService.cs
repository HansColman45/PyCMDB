using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace CMDB.API.Services
{
    public class ApplicationService : LogService, IApplicationService
    {
        public ApplicationService(CMDBContext context) : base(context)
        {
        }

        public Task<List<ApplicationDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<ApplicationDTO>> GetAll(string searchStr)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationDTO> GetById(int id)
        {
            Application? application = await _context.Applications.Where(x => x.AppID == id).FirstOrDefaultAsync();
            return ConvertApplication(application);
        }
        private ApplicationDTO ConvertApplication(Application application)
        {
            return new()
            {
                Active = application.active,
                AppID = application.AppID,
                DeactivateReason = application.DeactivateReason,
                LastModifiedAdminId = application.LastModifiedAdminId,
                Name = application.Name
            };
        }
    }
}
