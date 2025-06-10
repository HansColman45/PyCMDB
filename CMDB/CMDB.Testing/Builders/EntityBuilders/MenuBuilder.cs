using CMDB.Domain.Entities;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class MenuBuilder : GenericBogusEntityBuilder<Menu>
    {
        public MenuBuilder()
        {
            SetDefaultRules((f, m) =>
            {
                m.URL = "#";
                m.Label = f.Company.CompanyName();
            });
        }
    }
}
