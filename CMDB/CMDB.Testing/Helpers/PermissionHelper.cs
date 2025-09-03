using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.Testing.Builders.EntityBuilders;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class PermissionHelper
    {
        public static async Task<Permission> CreateSimplePermission(CMDBContext context, Admin admin)
        {
            Permission permission = new()
            {
                Rights = "TestRight",
                Description = "TestDescription",
                LastModifiedAdminId = admin.AdminId
            };
            context.Permissions.Add(permission);
            permission.Logs.Add(new LogBuilder()
                .With(x => x.Permission, permission)
                .With(x => x.LogText, $"The Permission with Rights: {permission.Rights} and Description: {permission.Description} is created by Automation in table permission")
                .Build());
            await context.SaveChangesAsync();
            return permission;
        }

        public static async Task Delete(CMDBContext context, Permission permission)
        {
            context.RemoveRange(permission.Logs);
            context.Permissions.Remove(permission);
            await context.SaveChangesAsync();
        }
    }
}
