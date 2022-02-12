using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using CMDB.Testing.Builders.EntityBuilders.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers.Devices
{
    public class DesktopHelper
    {
        public async static Task<Desktop> CreateSimpleDesktop(CMDBContext context, Admin admin, bool active = true)
        {
            var cat = context.AssetCategories.Where(x => x.Category == "Desktop").SingleOrDefault();
            var AssetType = await AssetTypeHelper.CreateSimpleAssetType(context, cat, admin);
            Desktop desktop = new DesktopBuilder()
                .With(x => x.Category, cat)
                .With(x => x.Type, AssetType)
                .With(x => x.LastModfiedAdmin, admin)
                .Build();
            desktop.Logs.Add(new LogBuilder()
                .With(x => x.Device, desktop)
                .With(x => x.LogText, $"The {cat.Category} with type {desktop.Type} is created by Automation in table laptop")
                .Build()
                );
            context.Devices.Add(desktop);
            await context.SaveChangesAsync();
            if (!active)
            {
                desktop.active = 0;
                context.SaveChanges();
            }
            return desktop;
        }
    }
}
