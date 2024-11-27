using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using CMDB.Testing.Builders.EntityBuilders.Devices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers.Devices
{
    public class DockingHelpers
    {
        public static async Task<Docking> CreateSimpleDocking(CMDBContext context, Admin admin, bool active = true)
        {
            var cat = context.AssetCategories.Where(x => x.Category == "Docking station").AsNoTracking().SingleOrDefault();
            var AssetType = await AssetTypeHelper.CreateSimpleAssetType(context, cat, admin);
            Docking docking = new DockingBuilder()
                .With(x => x.CategoryId, cat.Id)
                .With(x => x.TypeId, AssetType.TypeID)
                .With(x => x.LastModifiedAdminId, admin.AdminId)
                .With(x => x.IdentityId, 1)
                .Build();
            docking.Logs.Add(new LogBuilder()
                .With(x => x.Device, docking)
                .With(x => x.LogText, $"The {cat.Category} with type {docking.Type} is created by Automation in table docking")
                .Build()
                );
            context.Devices.Add(docking);
            await context.SaveChangesAsync();
            if (!active)
            {
                docking.active = 0;
                context.SaveChanges();
            }
            return docking;
        }
        public static async Task Delete(CMDBContext context, Docking docking)
        {
            context.RemoveRange(docking.Logs);
            context.Remove(docking);
            await context.SaveChangesAsync();
        }
    }
}
