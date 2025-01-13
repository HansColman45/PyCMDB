using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CMDB.Infrastructure
{
    public class CMDBContextDesignTimeFactory : IDesignTimeDbContextFactory<CMDBContext>
    {
        public CMDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CMDBContext>();
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CMDB;trusted_connection=true;User Id=sa;Password=Gr7k6VKW92dteZ5n;Encrypt=False;");
            return new CMDBContext(optionsBuilder.Options);
        }
    }
}
