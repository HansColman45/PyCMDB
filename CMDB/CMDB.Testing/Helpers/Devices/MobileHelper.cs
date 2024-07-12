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
    public class MobileHelper
    {
        public async static Task<Mobile> CreateSimpleMobile(CMDBContext context, Admin admin, bool active = true)
        {
            var cat = context.AssetCategories.Where(x => x.Category == "Mobile").SingleOrDefault();
            var AssetType = await AssetTypeHelper.CreateSimpleAssetType(context, cat, admin);

            Mobile mobile = new MobileBuilder()
                .With(x => x.Category, cat)
                .With(x => x.MobileType, AssetType)
                .With(x => x.LastModfiedAdmin, admin)
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
    }
}
