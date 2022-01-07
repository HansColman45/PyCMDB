using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using CMDB.Testing.Builders.EntityBuilders;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class LaptopHelper
    {
        public static async Task<Laptop> CreateSimpleLaptop(CMDBContext context, Admin admin)
        {
            var cat = context.AssetCategories.Where(x => x.Category == "Laptop").SingleOrDefault();
            var AssetType = await AssetTypeHelper.CreateSimpleAssetType(context, cat, admin);

            Laptop laptop = new LaptopBuilder()
                .With(x => x.Category, cat)
                .With(x => x.Type, AssetType)
                .With(x => x.LastModfiedAdmin, admin)
                .Build();
            laptop.Logs.Add(new LogBuilder()
                .With(x => x.Laptop, laptop)
                .With(x => x.LogText, $"The {cat.Category} with type {laptop.Type} is created by Automation in table laptop")
                .Build()
                );
            context.Laptops.Add(laptop);
            await context.SaveChangesAsync();

            return laptop;
        }
        public static async void Delete(CMDBContext context, Laptop laptop)
        {
            context.RemoveRange(laptop.Logs);
            context.Remove<Laptop>(laptop);
            await context.SaveChangesAsync();
        }
    }
}
