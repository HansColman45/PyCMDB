using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Infrastructure
{
    public class CMDBContextDesignTimeFactory : IDesignTimeDbContextFactory<CMDBContext>
    {
        public CMDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CMDBContext>();
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CMDB;trusted_connection=true;User Id=sa;Password=Gr7k6VKW92dteZ5n");
            return new CMDBContext(optionsBuilder.Options);
        }
    }
}
