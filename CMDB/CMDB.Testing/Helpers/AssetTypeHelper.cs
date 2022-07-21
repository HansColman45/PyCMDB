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
        public static async Task<AssetType> CreateSimpleAssetType(CMDBContext context, AssetCategory category, Admin admin, bool active = true)
        {
            try
            {
                var assetType = new AssetTypeBuilder()
                .With(x => x.Category, category)
                .With(x => x.LastModfiedAdmin, admin)
                .Build();

                assetType.Logs.Add(new LogBuilder()
                    .With(x => x.AssetType, assetType)
                    .With(x => x.LogText, $"The {category.Category} type Vendor: {assetType.Vendor} and type {assetType.Type} is created by Automation in table assettype")
                    .Build()
                );
                context.AssetTypes.Add(assetType);
            
                await context.SaveChangesAsync();
            
                if (!active)
                {
                    assetType.active = 0;
                    await context.SaveChangesAsync();
                }

                return assetType;
            }
            catch (Exception ex)
            {
                ex.ToString();
                throw;
            }
        }
        public static async Task Delete(CMDBContext context, AccountType accountType)
        {
            context.RemoveRange(accountType.Logs);
            context.Remove(accountType);
            await context.SaveChangesAsync();
        }
    }
}
