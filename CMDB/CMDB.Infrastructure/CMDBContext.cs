using Microsoft.EntityFrameworkCore;
using CMDB.Domain.Entities;

namespace CMDB.Infrastructure
{
    public class CMDBContext : DbContext
    {
        public CMDBContext(DbContextOptions<CMDBContext> options) : base(options)
        {
        }
        public Admin Admin { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<AssetCategory> AssetCategories { get; set; }
        public virtual DbSet<AssetType> AssetTypes { get; set; }
        public virtual DbSet<Laptop> Laptops { get; set; }
        public virtual DbSet<Desktop> Desktops { get; set; }
        public virtual DbSet<Screen> Screens { get; set; }
        public virtual DbSet<Mobile> Mobiles { get; set; }
        public virtual DbSet<Docking> Dockings { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<Identity> Identities { get; set; }
        public virtual DbSet<IdenAccount> IdenAccounts { get; set; }
        public virtual DbSet<IdentityType> IdentityTypes { get; set; }
        public virtual DbSet<Kensington> Kensingtons { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleType> RoleTypes { get; set; }
        public virtual DbSet<RolePerm> RolePerms { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public virtual DbSet<CMDB.Domain.Entities.Configuration> Configurations { get; set; }
        public virtual DbSet<RAM> RAMs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CMDBContext).Assembly);
        }
        // protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer("server=.;database=CMDB;User Id=sa;Password=Gr7k6VKW92dteZ5n", b => b.MigrationsAssembly("CMDB"));
    }
}
