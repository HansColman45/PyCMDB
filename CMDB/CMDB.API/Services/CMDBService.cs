using CMDB.Infrastructure;

namespace CMDB.API.Services
{
    public class CMDBService
    {
        protected CMDBContext _context;
        public CMDBService(CMDBContext context)
        {
            _context = context;
        }
    }
}
