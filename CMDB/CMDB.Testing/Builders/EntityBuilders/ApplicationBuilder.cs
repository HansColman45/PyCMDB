using CMDB.Domain.Entities;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class ApplicationBuilder : GenericBogusEntityBuilder<Application>
    {
        public ApplicationBuilder()
        {
            SetDefaultRules((f, a) =>
            {
                a.Name = f.Company.CompanyName();
            });
        }
    }
}
