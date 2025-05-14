using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// The interface for the Configuration repository.
    /// </summary>
    public interface IConfigurationRepository
    {
        /// <summary>
        /// This will get the configuration for the given code and subcode
        /// </summary>
        /// <param name="request"><see cref="ConfigurationRequest"/></param>
        /// <returns><see cref="Configuration"/></returns>
        Task<Configuration> GetConfiguration(ConfigurationRequest request);
    }
    /// <summary>
    /// Class ConfigurationRepository.
    /// </summary>
    public class ConfigurationRepository : GenericRepository, IConfigurationRepository
    {
        private ConfigurationRepository()
        {
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public ConfigurationRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc/>
        public async Task<Configuration> GetConfiguration(ConfigurationRequest request)
        {
            return await _context.Configurations.AsNoTracking()
                .Where(x => x.Code == request.Code && x.SubCode == request.SubCode).AsNoTracking()
                .SingleOrDefaultAsync();
        }
    }
}
