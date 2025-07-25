using CMDB.Domain.Entities;

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
