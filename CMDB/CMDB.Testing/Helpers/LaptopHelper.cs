using CMDB.Infrastructure;
using CMDB.Domain.Entities;
using System.Linq;
using CMDB.Testing.Builders.EntityBuilders;

namespace CMDB.Testing.Helpers
{
    public class LaptopHelper
    {
        public static Laptop CreateSimpleLaptop(CMDBContext context)
        {
            var cat = context.AssetCategories.Where(x => x.Category == "Laptop").SingleOrDefault();
            var AssetType = AssetTypeHelper.CreateSimpleAssetType(context, cat);

            Laptop laptop = new LaptopBuilder()
                .With(x => x.Category, cat)
                .With(x => x.Type, AssetType)
                .Build();
            laptop.Logs.Add(new LogBuilder()
                .With(x => x.Laptop, laptop)
                .With(x => x.LogText, $"The {cat.Category} with type {laptop.Type} is created by Automation in table laptop")
                .Build()
                );
            context.Laptops.Add(laptop);
            context.SaveChanges();

            return laptop;
        }
        public static void CascadingDelete(CMDBContext context, Laptop laptop)
        {
            var logs = context.Logs.Where(x => x.Laptop.AssetTag == laptop.AssetTag).ToList();
            context.RemoveRange(logs);

            context.Remove<AssetType>(laptop.Type);

            context.Remove<Laptop>(laptop);
            context.SaveChanges();
        }
    }
}
