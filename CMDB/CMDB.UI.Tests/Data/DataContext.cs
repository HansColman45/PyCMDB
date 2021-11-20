using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Identity = CMDB.Domain.Entities.Identity;

namespace CMDB.UI.Tests.Data
{
    public class DataContext
    {
        CMDBContext context;
        public DataContext()
        {
            string connectionstring = Settings.ConnectionString;
            var options = new DbContextOptionsBuilder<CMDBContext>()
                .UseSqlServer(connectionstring)
                .Options;
            context = new CMDBContext(options);
        }
        public Admin CreateNewAdmin(int level = 9)
        {
            var admin = AdminHelper.CreateCMDBAdmin(context, level);
            return admin;
        }
        public Identity GetIdentity(int IdenId)
        {
            var iden = context.Identities
                .Include(x => x.Type)
                .Include(x => x.Language)
                .Where(x => x.IdenId == IdenId)
                .FirstOrDefault();
            return iden;
        }
    }
}
