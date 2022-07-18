using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using CMDB.Testing.Builders.EntityBuilders.Devices;

namespace CMDB.Testing.Helpers.Devices
{
    public class DockingHelpers
    {
        public static async Task<Docking> CreateSimpleDocking(CMDBContext context, Admin admin, bool active = true)
        {
            var cat = context.AssetCategories.Where(x => x.Category == "Docking station").SingleOrDefault();
            var AssetType = await AssetTypeHelper.CreateSimpleAssetType(context, cat, admin);
            Docking docking = new DockingBuilder()
                .With(x => x.Category, cat)
                .With(x => x.Type, AssetType)
                .With(x => x.LastModfiedAdmin, admin)
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
