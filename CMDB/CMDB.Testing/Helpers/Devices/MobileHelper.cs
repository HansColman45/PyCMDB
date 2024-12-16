using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using CMDB.Testing.Builders.EntityBuilders.Devices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers.Devices
{
    public class MobileHelper
    {
        public async static Task<Mobile> CreateSimpleMobile(CMDBContext context, Admin admin, bool active = true)
        {
            var cat = context.AssetCategories.Where(x => x.Category == "Mobile").AsNoTracking().SingleOrDefault();
            var AssetType = await AssetTypeHelper.CreateSimpleAssetType(context, cat, admin);

            Mobile mobile = new MobileBuilder()
                .With(x => x.CategoryId, cat.Id)
                .With(x => x.TypeId, AssetType.TypeID)
                .With(x => x.LastModifiedAdminId, admin.AdminId)
                .With(x => x.IdentityId, 1)
                .Build();
            mobile.Logs.Add(new LogBuilder().With(x => x.Mobile, mobile)
                .With(x => x.LogText, $"The {cat.Category} with type {mobile.MobileType} is created by Automation in table mobile")
                .Build()
                );
            context.Mobiles.Add(mobile);
            await context.SaveChangesAsync();
            if (!active)
            {
                mobile.Active = State.Inactive;
                await context.SaveChangesAsync();
            }
            return mobile; 
        }
        public async static Task Delete(CMDBContext context, Mobile mobile)
        {
            foreach (var log in mobile.Logs)
            {
                context.Remove(log);
            }
            context.Remove(mobile);
            await context.SaveChangesAsync();
        }
    }
}
