using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class AssetTypeHelper
    {
        public static AssetType CreateSimpleAssetType(CMDBContext context, AssetCategory category)
        {
            var assetType = new AssetTypeBuilder()
                .With(x => x.Category, category)
                .Build();
            context.AssetTypes.Add(assetType);
            context.SaveChanges();
            return assetType;
        }
    }
}
