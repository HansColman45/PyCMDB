using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using System;
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
                    .With(x => x.CategoryId, category.Id)
                    .With(x => x.LastModifiedAdminId, admin.AdminId)
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
        public static async Task Delete(CMDBContext context, AssetType assetType)
        {
            //context.RemoveRange(assetType.Logs);
            foreach (var log in assetType.Logs)
            {
                context.Remove(log);
            }
            context.Remove(assetType);
            await context.SaveChangesAsync();
        }
    }
}
