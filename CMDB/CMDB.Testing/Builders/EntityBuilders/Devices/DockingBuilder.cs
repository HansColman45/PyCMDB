using CMDB.Domain.Entities;

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
