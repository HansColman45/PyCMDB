using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class AdminHelper
    {
        public static async Task<Admin> CreateCMDBAdmin(CMDBContext context, int level = 9)
        {
            var app = context.Applications.Where(x => x.Name == "CMDB").FirstOrDefault();
            var language = context.Languages.Where(x => x.Code == "NL").FirstOrDefault();
            var identype = context.IdentityTypes.Where(x => x.Type == "Werknemer").FirstOrDefault();
            var accounttype = context.AccountTypes.Where(x => x.Type == "Administrator").FirstOrDefault();

            var Account = new AccountBuilder()
                .With(x => x.Application, app)
                .With(x => x.Type, accounttype)
                .With(x => x.LastModifiedAdminId, 1)
                .Build();

            Account.Logs.Add(new LogBuilder()
                .With(x => x.Account, Account)
                .With(x => x.LogText, $"Account created {Account.UserID} for application {Account.Application.Name}")
                .Build()
            );
            context.Accounts.Add(Account);

            var admin = new AdminBuilder()
                .With(x => x.Level, level)
                .With(x => x.Account, Account)
                .With(x => x.LastModifiedAdminId, 1)
                .Build();

            admin.Logs.Add(new LogBuilder()
                .With(x => x.Admin, admin)
                .With(x => x.LogText, $"Admin created with userid: {admin.Account.UserID}")
                .Build()
            );

            context.Admins.Add(admin);
            await context.SaveChangesAsync();

            var identity = new IdentityBuilder()
                .With(x => x.Language, language)
                .With(x => x.Type, identype)
                .With(x => x.UserID, Account.UserID)
                .With(x => x.LastModifiedAdminId, 1)
                .Build();

            identity.Logs.Add(new LogBuilder()
                .With(x => x.Identity, identity)
                .With(x => x.LogText, $"identity created {identity.Name}")
                .Build()
            );

            context.Identities.Add(identity);

            context.IdenAccounts.Add(new()
            {
                Identity = identity,
                Account = Account,
                LastModifiedAdminId = 1
            });

            await context.SaveChangesAsync();
            return admin;
        }
        public static async Task DeleteCascading(CMDBContext context, Admin admin)
        {
            //AccountType
            var accountType = context.AccountTypes
                .Include(x => x.Logs)
                .Where(x => x.LastModifiedAdminId == admin.AdminId)
                .ToList();
            foreach (var type in accountType)
            {
                await AssetTypeHelper.Delete(context, type);
            }
            //IdentityTypes
            var identypes = context.IdentityTypes
                .Include(x => x.Logs)
                .Where(x => x.LastModifiedAdminId == admin.AdminId)
                .ToList();
            foreach (var type in identypes)
            {
                await IdentityTypeHelper.Delete(context, type);
            }
            //Laptop
            var laptops = context.Laptops
                .Include(x => x.Logs)
                .Where(x => x.LastModifiedAdminId == admin.AdminId)
                .ToList();
            foreach (var laptop in laptops)
            {
                await LaptopHelper.Delete(context, laptop);
            }
            //IdenAccount
            var idenAccs = context.IdenAccounts
                .Where(x => x.LastModifiedAdminId == admin.AdminId)
                .ToList();
            foreach (var idenacc in idenAccs)
            {
                context.Remove<IdenAccount>(idenacc);
            }
            //Identity
            var identities = context.Identities
                .Include(x => x.Logs)
                .Where(x => x.LastModifiedAdminId == admin.AdminId)
                .ToList();
            foreach (var identity in identities)
            {
                await IdentityHelper.Delete(context, identity);
            }
            //Account
            var accounts = context.Accounts
                .Include(x => x.Logs)
                .Where(x => x.LastModifiedAdminId == admin.AdminId)
                .ToList();
            foreach (var account in accounts)
            {
                await AccountHelper.Delete(context, account);
            }
            //Admin
            context.RemoveRange(admin.Logs);
            context.Remove<Admin>(admin);
            await context.SaveChangesAsync();
        }
    }
}
