using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using System;
using System.Threading.Tasks;

namespace CMDB.Testing.Helpers
{
    public class MenuHelper
    {
        public static async Task<Menu> CreateSimpleMenu(CMDBContext context, Admin admin)
        {
            Menu menu = new()
            {
                Label = "Test Menu",
                URL = "#"
            };
            menu.Logs.Add(new Log
            {
                Menu = menu,
                LogText = $"The Menu with Label: {menu.Label} and URL: {menu.URL} is created by Automation in table Menu",
            });
            context.Menus.Add(menu);
            await context.SaveChangesAsync();
            return menu;
        }

        public static async Task Delete(CMDBContext context, Menu menu)
        {
            foreach(var log in menu.Logs)
            {
                context.Logs.Remove(log);
            }
            context.Menus.Remove(menu);
            await context.SaveChangesAsync();
        }
    }
}
