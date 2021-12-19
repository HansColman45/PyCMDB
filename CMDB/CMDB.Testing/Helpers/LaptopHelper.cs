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

            Laptop laptop = new LaptopBuilder()
                .With(x => x.Category, cat)
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
    }
}
