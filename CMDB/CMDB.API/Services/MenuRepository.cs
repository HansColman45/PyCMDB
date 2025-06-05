using CMDB.API.Interfaces;
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// Class for Menu Repository
    /// </summary>
    public class MenuRepository : GenericRepository, IMenuRepository
    {
        private MenuRepository()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public MenuRepository(CMDBContext context, ILogger logger) : base(context, logger)
        {
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<Menu>> GetAll()
        {
            return await _context.Menus.AsNoTracking().ToListAsync();
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<Menu>> GetFirstLevel()
        {
            return await _context.Menus.AsNoTracking()
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.ParentId).ThenBy(x => x.MenuId)
                .ToListAsync();
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<Menu>> GetSecondLevel(int id)
        {
            return await _context.Menus.Include(x => x.Parent).AsNoTracking()
                .Where(x => x.ParentId == id)
                .OrderBy(x => x.ParentId).ThenBy(x => x.MenuId)
                .ToListAsync();
        }
        /// <inheritdoc/>
        public async Task<IEnumerable<Menu>> GetPestonalMenu(int menuId, int level)
        {
            return await _context.RolePerms
                .Include(x => x.Menu)
                .ThenInclude(x => x.Children)
                .Include(x => x.Permission)
                .Where(x => x.Permission.Rights == "Read" && x.Level == level && x.Menu.MenuId == menuId).AsNoTracking()
                .SelectMany(x => x.Menu.Children).ToListAsync();
        }
        /// <inheritdoc/>
        public Task<Menu> GetById(int id)
        {
            return _context.Menus.AsNoTracking()
                .FirstOrDefaultAsync(x => x.MenuId == id);
        }
    }
}
