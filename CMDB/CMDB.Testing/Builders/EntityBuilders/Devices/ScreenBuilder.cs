using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders.Devices
{
    public class ScreenBuilder: GenericBogusEntityBuilder<Screen>
    {
        public ScreenBuilder()
        {
            SetDefaultRules((f, s) =>
            {
                s.AssetTag = "SRC"+f.Address.ZipCode();
                s.SerialNumber = f.Commerce.Ean8();
            });
        }
    }
}
