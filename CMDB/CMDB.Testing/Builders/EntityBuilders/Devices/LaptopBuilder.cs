using CMDB.Domain.Entities;

namespace CMDB.Testing.Builders.EntityBuilders.Devices
{
    class LaptopBuilder : GenericBogusEntityBuilder<Laptop>
    {
        public LaptopBuilder()
        {
            SetDefaultRules((f, l) =>
            {
                l.AssetTag = "LPT" + f.Address.ZipCode();
                l.RAM = "128";
                l.MAC = "";
                l.SerialNumber = f.Commerce.Ean8();
            });
        }
    }
}
