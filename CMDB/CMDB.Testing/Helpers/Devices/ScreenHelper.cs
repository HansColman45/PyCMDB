using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using CMDB.Testing.Builders.EntityBuilders.Devices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers.Devices
{
    public class ScreenHelper
    {
        public static async Task<Screen> CreateScreen(CMDBContext context, Admin admin, bool active = true)
        {
            var cat = context.AssetCategories.Where(x => x.Category == "Monitor").AsNoTracking().SingleOrDefault();
            var AssetType = await AssetTypeHelper.CreateSimpleAssetType(context, cat, admin);
            Screen screen = new ScreenBuilder()
                .With(x => x.CategoryId, cat.Id)
                .With(x => x.TypeId, AssetType.TypeID)
                .With(x => x.LastModifiedAdminId, admin.AdminId)
                .With(x => x.IdentityId, 1)
                .Build();
            screen.Logs.Add(new LogBuilder().With(x => x.Device, screen)
                .With(x => x.LogText, $"The {cat.Category} with type {screen.Type} is created by Automation in table screen")
                .Build()
                );
            context.Devices.Add(screen);
            await context.SaveChangesAsync();
            if (!active)
            {
                screen.active = 0;
                context.SaveChanges();
            }
            return screen;
        }
        public static async Task Delete(CMDBContext context, Screen screen)
        {
            context.RemoveRange(screen.Logs);
            context.Remove(screen);
            await context.SaveChangesAsync();
        }
    }
}
