using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class KensingtonHelper
    {
        public static async Task<Kensington> CreateKensington(CMDBContext context, Admin admin, bool active = true)
        {
            var cat = context.AssetCategories.Where(x => x.Category == "Kensington").AsNoTracking().SingleOrDefault();
            var AssetType = await AssetTypeHelper.CreateSimpleAssetType(context, cat, admin);
            Kensington kensington = new KensingtonBuilder()
                .With(x => x.LastModifiedAdminId, admin.AdminId)
                .With(x => x.TypeId, AssetType.TypeID)
                .With(x => x.CategoryId, cat.Id)
                .Build();
            context.Kensingtons.Add(kensington);
            kensington.Logs.Add(new LogBuilder()
                .With(x => x.Kensington, kensington)
                .With(x => x.LogText, $"The Kensington with serial number: {kensington.SerialNumber} is created by Automation in table kensington")
                .Build()
            );
            await context.SaveChangesAsync();
            if (!active)
            {
                kensington.Active = 0;
                await context.SaveChangesAsync();
            }
            return kensington;
        }
    }
}
