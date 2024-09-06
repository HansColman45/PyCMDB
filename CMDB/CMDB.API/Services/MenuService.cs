using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public class MenuService : CMDBService, IMenuService
    {
        private ILogService _logService;
        public MenuService(CMDBContext context, ILogService logService) : base(context)
        {
            _logService = logService;
        }

        public async Task<ICollection<Menu>> ListFirstMenuLevel()
        {
            var menu = await _context.Menus.Where(x => x.ParentId == null)
                .OrderBy(x => x.ParentId).ThenBy(x => x.MenuId)
                .ToListAsync();
            return menu;
        }
        public async Task<ICollection<Menu>> ListPersonalMenu(int level, int menuID)
        {
            var menu = await _context.RolePerms
                .Include(x => x.Menu)
                .ThenInclude(x => x.Children)
                .Include(x => x.Permission)
                .Where(x => x.Permission.Rights == "Read" && x.Level == level && x.Menu.MenuId == menuID)
                .SelectMany(x => x.Menu.Children).ToListAsync();
            return menu;
        }
        public async Task<ICollection<Menu>> ListSecondMenuLevel(int menuID)
        {
            var menu = await _context.Menus
                .Include(x => x.Parent)
                .Where(x => x.ParentId == menuID)
                .OrderBy(x => x.ParentId).ThenBy(x => x.MenuId)
                .ToListAsync();
            return menu;
        }
    }
}
