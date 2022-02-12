using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders.Devices
{
    class LaptopBuilder : GenericBogusEntityBuilder<Laptop>
    {
        public LaptopBuilder()
        {
            SetDefaultRules((f, l) =>
            {
                l.AssetTag = "LPT" + f.Address.ZipCode();
                l.Type = new AssetTypeBuilder().Build();
                l.RAM = "128";
                l.MAC = "";
                l.SerialNumber = f.Commerce.Ean8();
            });
        }
    }
}
