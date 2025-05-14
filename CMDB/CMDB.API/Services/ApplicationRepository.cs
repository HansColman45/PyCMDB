using CMDB.API.Models;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// Repository for managing Application entities.
    /// </summary>
    public class ApplicationRepository : GenericRepository, IApplicationRepository
    {
        private ApplicationRepository()
        {
        }

        private readonly string table = "application";
        /// <summary>
        /// Constructor for ApplicationRepository.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public ApplicationRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc />
        public async Task<IEnumerable<ApplicationDTO>> GetAll()
        {
            var applications = await _context.Applications.AsNoTracking()
                .Select(x => ConvertApplication(x))
                .ToListAsync();
            return applications;
        }
        /// <inheritdoc />
        public async Task<IEnumerable<ApplicationDTO>> GetAll(string searchStr)
        {
            string searhterm = "%" + searchStr + "%";
            var applications = await _context.Applications.AsNoTracking()
                .Where(x => EF.Functions.Like(x.Name,searhterm)).AsNoTracking()
                .Select(x => ConvertApplication(x))
                .ToListAsync();
            return applications;
        }
        /// <inheritdoc />
        public async Task<ApplicationDTO> GetById(int Id)
        {
            _logger.LogInformation("Get information about ApplicationDTO by Id: {Id}",Id);
            return await _context.Applications.AsNoTracking()
                .Where(x => x.AppID == Id).AsNoTracking()
                .Select(x => ConvertApplication(x))
                .FirstOrDefaultAsync();
        }
        /// <inheritdoc />
        public async Task<Application> Update(ApplicationDTO appDTO)
        {
            var oldApp = await GetAppById(appDTO.AppID);
            var newApp = ConvertDTO(appDTO);
            if(string.Compare(oldApp.Name,newApp.Name) != 0)
            {
                oldApp.Name = newApp.Name;
                newApp.Logs.Add(new()
                {
                    LogText = GenericLogLineCreator.UpdateLogLine("name", oldApp.Name, newApp.Name, TokenStore.Admin.Account.UserID, table),
                    LogDate = DateTime.Now
                });
            }
            _context.Applications.Update(oldApp);
            return oldApp;
        }
        /// <inheritdoc />
        public async Task<Application> DeActivate(ApplicationDTO application, string reason)
        {
            var app = await GetAppById(application.AppID);
            app.DeactivateReason = reason;
            app.Active = State.Inactive;
            app.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.DeleteLogLine($"application with name {app.Name}", TokenStore.Admin.Account.UserID, reason,table),
                LogDate= DateTime.Now
            });
            _context.Applications.Update(app);
            return app;
        }
        /// <inheritdoc />
        public async Task<Application> Activate(ApplicationDTO application)
        {
            var app = await GetAppById(application.AppID);
            app.Active = State.Active;
            app.DeactivateReason = "";
            app.Logs.Add(new()
            {
                LogText = GenericLogLineCreator.ActivateLogLine($"application with name {app.Name}", TokenStore.Admin.Account.UserID, table),
                LogDate = DateTime.Now
            });
            _context.Applications.Update(app);
            return app;
        }
        /// <summary>
        /// Converts an Application entity to an ApplicationDTO.
        /// </summary>
        /// <param name="application"><see cref="Application"/></param>
        /// <returns><see cref="ApplicationDTO"/></returns>
        public static ApplicationDTO ConvertApplication(in Application application)
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
        /// <summary>
        /// Converts an ApplicationDTO to an Application entity.
        /// </summary>
        /// <param name="dto"><see cref="ApplicationDTO"/></param>
        /// <returns><see cref="Application"/></returns>
        public static Application ConvertDTO(ApplicationDTO dto)
        {
            return new()
            {
                AppID = dto.AppID,
                active = dto.Active,
                DeactivateReason = dto.DeactivateReason,
                LastModifiedAdminId = dto.LastModifiedAdminId,
                Name = dto.Name,
            };
        }
        private async Task<Application> GetAppById(int Id)
        {
            return await _context.Applications.FirstOrDefaultAsync(x => x.AppID == Id);
        }
    }
}
