using CMDB.Domain.Entities;

namespace CMDB.Testing.Builders.EntityBuilders
{
    class KensingtonBuilder : GenericBogusEntityBuilder<Kensington>
    {
        public KensingtonBuilder()
        {
            SetDefaultRules((f, k) =>
            {
                k.SerialNumber = f.Commerce.Ean8();
                k.AmountOfKeys = 1;
                k.HasLock = false;
            });
        }
    }
}
