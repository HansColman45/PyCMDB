using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class RolePermissionHelper
    {
        public static async Task<RolePerm> CreateDefaultRolePermission(CMDBContext context, Menu menu, Permission permission, Admin admin)
        {
            var rolePerm = new RolePermissionBuilder()
                .With(x => x.MenuId, menu.MenuId)
                .With(x => x.PermissionId, permission.Id)
                .With(x => x.LastModifiedAdminId, admin.AdminId)
                .Build();
            context.RolePerms.Add(rolePerm);
            rolePerm.Logs.Add(new LogBuilder()
                .With(x => x.RolePerm, rolePerm)
                .With(x => x.LogText, $"The Role Permission with MenuId: {menu.MenuId} and PermissionId: {permission.Id} is created by Automation in table RolePerm")
                .Build()
            );
            await context.SaveChangesAsync();
            return rolePerm;
        }
        public static async Task Delete(CMDBContext context, RolePerm rolePerm)
        {
            foreach(var log in rolePerm.Logs)
            {
                context.Remove(log);
            }
            context.Remove(rolePerm);
            await context.SaveChangesAsync();
        }
    }
}
