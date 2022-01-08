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
    public class AssetTypeHelper
    {
        public static async Task<AssetType> CreateSimpleAssetType(CMDBContext context, AssetCategory category, Admin admin)
        {
            var assetType = new AssetTypeBuilder()
                .With(x => x.Category, category)
                .With(x => x.LastModfiedAdmin, admin)
                .Build();
            context.AssetTypes.Add(assetType);
            await context.SaveChangesAsync();
            return assetType;
        }
        public static async Task Delete(CMDBContext context, AccountType accountType)
        {
            context.RemoveRange(accountType.Logs);
            context.Remove<AccountType>(accountType);
            await context.SaveChangesAsync();
        }
    }
}
