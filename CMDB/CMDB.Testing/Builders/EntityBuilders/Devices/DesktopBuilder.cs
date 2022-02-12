using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders.Devices
{
    class DesktopBuilder : GenericBogusEntityBuilder<Desktop>
    {
        public DesktopBuilder()
        {
            SetDefaultRules((f, d) =>
            {
                d.AssetTag = "DST" + f.Address.ZipCode();
                d.Type = new AssetTypeBuilder().Build();
                d.RAM = "128";
                d.MAC = "";
                d.SerialNumber = f.Commerce.Ean8();
            });
        }
    }
}
