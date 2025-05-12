using CMDB.Domain.Entities;
using CMDB.Domain.Requests;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface IConfigurationRepository
    {
        Task<Configuration> GetConfiguration(ConfigurationRequest request);
    }
    public class ConfigurationRepository : GenericRepository, IConfigurationRepository
    {
        public ConfigurationRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task<Configuration> GetConfiguration(ConfigurationRequest request)
        {
            return await _context.Configurations.AsNoTracking()
                .Where(x => x.Code == request.Code && x.SubCode == request.SubCode).AsNoTracking()
                .SingleOrDefaultAsync();
        }
    }
}
