using CMDB.API.Helper;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CMDB.API.Services
{
    public class IdentityService : LogService, IIdentityService
    {
        private readonly AppSettings _appSettings;
        private readonly CMDBContext _context;

        public IdentityService(IOptions<AppSettings> settings, CMDBContext context) : base(context)
        {
            _appSettings = settings.Value;
            _context = context;
        }
        public async Task<IEnumerable<Identity>> GetAll()
        {
            return await _context.Identities.ToListAsync();
        }
        public async Task<Identity?> GetById(int id)
        {
            return await _context.Identities.SingleOrDefaultAsync(x => x.IdenId == id);
        }
        public async Task<Account?> GetAccountById(int id)
        {
            return await _context.Accounts.SingleOrDefaultAsync(x => x.AccID == id);
        }
    }
}
