using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class IdentityService : LogService, IIdentityService
    {

        public IdentityService(CMDBContext context) : base(context)
        {
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
