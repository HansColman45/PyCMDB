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
    public class AccountTypeHelper
    {
        public static async Task<AccountType> CreateSimpleAccountType(CMDBContext context, Admin admin, bool active = true)
        {
            AccountType accountType = new AccountTypeBuilder()
                .With(x => x.LastModifiedAdminId, admin.AdminId)
                .Build();
            context.Types.Add(accountType);
            accountType.Logs.Add(new LogBuilder()
                .With(x => x.Type, accountType)
                .With(x => x.LogText, $"The IdentityType with type: {accountType.Type} and description: {accountType.Description} is created by Automation in table accounttype")
                .Build()
            );
            await context.SaveChangesAsync();
            if (!active)
            {
                accountType.Active = State.Inactive;
                await context.SaveChangesAsync();
            }
            return accountType;
        }
        public static async Task Delete(CMDBContext context, AccountType accountType)
        {
            context.RemoveRange(accountType.Logs);
            context.Remove(accountType);
            await context.SaveChangesAsync();
        }
    }
}
