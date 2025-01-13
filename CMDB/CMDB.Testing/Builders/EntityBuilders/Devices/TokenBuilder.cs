using CMDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.Testing.Builders.EntityBuilders.Devices
{
    public class TokenBuilder: GenericBogusEntityBuilder<Token>
    {
        public TokenBuilder()
        {
            SetDefaultRules((f, t) =>
            {
                t.AssetTag = "TKN"+f.Address.ZipCode();
                t.SerialNumber = f.Commerce.Ean8();
            });
        }
    }
}
