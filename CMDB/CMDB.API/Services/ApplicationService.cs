using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace CMDB.API.Services
{
    public class ApplicationService : CMDBService, IApplicationService
    {
        private readonly ILogger<ApplicationService> _logger;
        private ILogService _logService;
        public ApplicationService(CMDBContext context, ILogService logService, ILogger<ApplicationService> logger) : base(context)
        {
            _logger = logger;
            _logService = logService;
        }

        public Task<List<ApplicationDTO>> GetAll()
        {
            var applications = _context.Applications
                .Select(x => new ApplicationDTO()
                {
                    AppID = x.AppID,
                    Active = x.active,
                    DeactivateReason = x.DeactivateReason,
                    LastModifiedAdminId = x.LastModifiedAdminId,
                    Name = x.Name,
                })
                .ToListAsync();
            return applications;
        }

        public Task<List<ApplicationDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            var applications = _context.Applications
                .Select(x => new ApplicationDTO()
                {
                    AppID = x.AppID,
                    Active = x.active,
                    DeactivateReason = x.DeactivateReason,
                    LastModifiedAdminId = x.LastModifiedAdminId,
                    Name = x.Name,
                })
                .ToListAsync();
            return applications;
        }

        public async Task<ApplicationDTO> GetById(int id)
        {
            Application? application = await _context.Applications.Where(x => x.AppID == id).FirstOrDefaultAsync();
            return ConvertApplication(application);
        }
        public static ApplicationDTO ConvertApplication(Application application)
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
