using CMDB.Domain.Entities;

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
