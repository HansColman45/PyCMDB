using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders.Devices
{
    class DockingBuilder : GenericBogusEntityBuilder<Docking>
    {
        public DockingBuilder()
        {
            SetDefaultRules((f, d) =>
            {
                d.AssetTag = "DOC" + f.Address.ZipCode();
                d.SerialNumber = f.Commerce.Ean8();
            });
        }
    }
}
