using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders.Devices
{
    public class MobileBuilder : GenericBogusEntityBuilder<Mobile>
    {
        public MobileBuilder()
        {
            SetDefaultRules((f,m) =>
            {
                m.IMEI = Convert.ToInt64(f.Commerce.Ean13());
            });
        }
    }
}
