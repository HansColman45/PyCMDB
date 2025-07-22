using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CMDB.Infrastructure
{
    public class CMDBContextFactory : IDesignTimeDbContextFactory<CMDBContext>
    {
        public CMDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CMDBContext>();
            optionsBuilder.UseSqlServer("server=.;database=CMDB;User Id=sa;Password=Gr7k6VKW92dteZ5n;Encrypt=False");
            return new CMDBContext(optionsBuilder.Options);
        }
    }
}
