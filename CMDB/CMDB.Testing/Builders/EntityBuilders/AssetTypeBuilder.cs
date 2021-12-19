using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders
{
    class AssetTypeBuilder : GenericBogusEntityBuilder<AssetType>
    {
        public AssetTypeBuilder()
        {
            SetDefaultRules((f, at) =>
            {
                at.Vendor = f.Commerce.Product();
                at.Type = f.Commerce.Ean8();
            });
        }
    }
}
