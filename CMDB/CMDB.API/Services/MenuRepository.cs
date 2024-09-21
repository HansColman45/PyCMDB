using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    public interface IMenuRepository
    {
        Task<IEnumerable<Menu>> GetFirstLevel();
        Task<IEnumerable<Menu>> GetSecondLevel(int id);
        Task<IEnumerable<Menu>> GetPestonalMenu(int menuId, int level);
    }
    public class MenuRepository : GenericRepository, IMenuRepository
    {
        public MenuRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        public async Task<IEnumerable<Menu>> GetFirstLevel()
        {
            return await _context.Menus.AsNoTracking()
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.ParentId).ThenBy(x => x.MenuId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Menu>> GetSecondLevel(int id)
        {
            return await _context.Menus.Include(x => x.Parent).AsNoTracking()
                .Where(x => x.ParentId == id)
                .OrderBy(x => x.ParentId).ThenBy(x => x.MenuId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Menu>> GetPestonalMenu(int menuId, int level)
        {
            return await _context.RolePerms.AsNoTracking()
                .Include(x => x.Menu)
                .ThenInclude(x => x.Children).AsNoTracking()
                .Include(x => x.Permission).AsNoTracking()
                .Where(x => x.Permission.Rights == "Read" && x.Level == level && x.Menu.MenuId == menuId)
                .SelectMany(x => x.Menu.Children).ToListAsync();
        }
    }
}
