using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class AccountHelper
    {
        public static async Task<Account> CreateSimpleAccountAsync(CMDBContext context, Admin admin)
        {
            var accounttype = context.AccountTypes.Where(x => x.Type == "Administrator").FirstOrDefault();
            var app = context.Applications.Where(x => x.Name == "CMDB").FirstOrDefault();

            var Account = new AccountBuilder()
                .With(x => x.Application, app)
                .With(x => x.Type, accounttype)
                .With(x => x.LastModfiedAdmin, admin)
                .Build();

            Account.Logs.Add(new LogBuilder()
                .With(x => x.Account, Account)
                .With(x => x.LogText, $"The Account width UserID: {Account.UserID} with type {Account.Type} for application {Account.Application} is created by Automation in table account")
                .Build()
            );
            context.Accounts.Add(Account);
            await context.SaveChangesAsync();
            return Account;
        }
        public static async Task Delete(CMDBContext context, Account account)
        {
            context.RemoveRange(account.Logs);
            context.Remove<Account>(account);
            await context.SaveChangesAsync();
        }
    }
}
