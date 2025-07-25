using CMDB.Domain.Entities;

namespace CMDB.Testing.Builders.EntityBuilders.Devices
{
    class DesktopBuilder : GenericBogusEntityBuilder<Desktop>
    {
        public DesktopBuilder()
        {
            SetDefaultRules((f, d) =>
            {
                d.AssetTag = "DST" + f.Address.ZipCode();
                d.RAM = "128";
                d.MAC = "";
                d.SerialNumber = f.Commerce.Ean8();
            });
        }
    }
}
