using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMDB.Testing.Helpers
{
    public class AdminHelper
    {
        public static Admin CreateCMDBAdmin(CMDBContext context, int level = 9)
        {
            var app = context.Applications.Where(x => x.Name == "CMDB").FirstOrDefault();
            var language = context.Languages.Where(x => x.Code == "NL").FirstOrDefault();
            var identype = context.IdentityTypes.Where(x => x.Type == "Werknemer").FirstOrDefault();
            var accounttype = context.AccountTypes.Where(x => x.Type == "Administrator").FirstOrDefault();

            var Account = new AccountBuilder()
                .With(x => x.Application, app)
                .With(x => x.Type, accounttype)
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
                .Build();

            admin.Logs.Add(new LogBuilder()
                .With(x => x.Admin, admin)
                .With(x => x.LogText, $"Admin created with userid: {admin.Account.UserID}")
                .Build()
            );

            context.Admins.Add(admin);
            context.SaveChanges();

            var identity = new IdentityBuilder()
                .With(x => x.Language, language)
                .With(x => x.Type, identype)
                .With(x => x.UserID, Account.UserID)
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
                Account = Account
            });

            context.SaveChanges();
            return admin;
        }
        public static void DeleteCascading(CMDBContext context, Admin admin)
        {
            //AccountType
            var accountType = context.AccountTypes
                .Include(x => x.Logs)
                .Where(x => x.LastModifiedAdminId == admin.AdminId)
                .ToList();
            foreach (var type in accountType)
            {
                context.RemoveRange(type.Logs);
                context.SaveChanges();
            }
            context.RemoveRange(accountType);
            context.SaveChanges();
            //IdentityTypes
            var identypes = context.IdentityTypes
                .Include(x => x.Logs)
                .Where(x => x.LastModifiedAdminId == admin.AdminId)
                .ToList();
            foreach (var type in identypes)
            {
                context.RemoveRange(type.Logs);
                context.SaveChanges();
            }
            context.RemoveRange(identypes);
            context.SaveChanges();
            //Identity
            var identities = context.Identities
                .Include(x => x.Logs)
                .Where(x => x.LastModifiedAdminId == admin.AdminId)
                .ToList();
            foreach (var identity in identities)
            {
                context.RemoveRange(identity.Logs);
                context.SaveChanges();
            }
        }
    }
}
