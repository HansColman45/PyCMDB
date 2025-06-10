using CMDB.Domain.Entities;

namespace CMDB.Testing.Builders.EntityBuilders
{
    public class RolePermissionBuilder : GenericBogusEntityBuilder<RolePerm>
    {
        public RolePermissionBuilder()
        {
            SetDefaultRules((f,p) =>
            {
                p.Level = 9;
            });
        }
    }
}
