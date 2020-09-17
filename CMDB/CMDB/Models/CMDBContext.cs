using System.Data.Entity;

namespace CMDB.Models
{
    public class CMDBContext: DbContext
    {
        public string ConnectionString { get; set; }
        public CMDBContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Identity>().ToTable("identity");
        }
    }
}