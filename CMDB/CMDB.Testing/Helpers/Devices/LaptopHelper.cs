using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using CMDB.Testing.Builders.EntityBuilders.Devices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers.Devices
{
    public class LaptopHelper
    {
        public static async Task<Laptop> CreateSimpleLaptop(CMDBContext context, Admin admin, bool active = true)
        {
            try
            {
                var cat = await context.AssetCategories.Where(x => x.Category == "Laptop").FirstOrDefaultAsync();
                var AssetType = await AssetTypeHelper.CreateSimpleAssetType(context, cat, admin);

                Laptop laptop = new LaptopBuilder()
                    .With(x => x.Category, cat)
                    .With(x => x.Type, AssetType)
                    .With(x => x.LastModfiedAdmin, admin)
                    .With(x => x.IdentityId, 1)
                    .Build();
                laptop.Logs.Add(new LogBuilder()
                    .With(x => x.Device, laptop)
                    .With(x => x.LogText, $"The {cat.Category} with type {laptop.Type} is created by Automation in table laptop")
                    .Build()
                    );
                context.Devices.Add(laptop);
                await context.SaveChangesAsync();
                if (!active)
                {
                    laptop.active = 0;
                    await context.SaveChangesAsync();
                }
                return laptop;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public static async Task Delete(CMDBContext context, Laptop laptop)
        {
            context.RemoveRange(laptop.Logs);
            context.Remove(laptop);
            await context.SaveChangesAsync();
        }
    }
}
