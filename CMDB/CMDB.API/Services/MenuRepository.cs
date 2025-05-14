using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CMDB.API.Services
{
    /// <summary>
    /// The interface for menu
    /// </summary>
    public interface IMenuRepository
    {
        /// <summary>
        /// This will return a list of Menus
        /// </summary>
        /// <returns>List of <see cref="Menu"/></returns>
        Task<IEnumerable<Menu>> GetFirstLevel();
        /// <summary>
        /// This will return the 2nd level menu
        /// </summary>
        /// <param name="id">The Id of the parent Menu</param>
        /// <returns>List of <see cref="Menu"/></returns>
        Task<IEnumerable<Menu>> GetSecondLevel(int id);
        /// <summary>
        /// This returns the personal menu
        /// </summary>
        /// <param name="menuId">The Id of the parent Menu</param>
        /// <param name="level">The level of the Admin</param>
        /// <returns>List of <see cref="Menu"/></returns>
        Task<IEnumerable<Menu>> GetPestonalMenu(int menuId, int level);
    }
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
            return await _context.RolePerms.AsNoTracking()
                .Include(x => x.Menu)
                .ThenInclude(x => x.Children).AsNoTracking()
                .Include(x => x.Permission).AsNoTracking()
                .Where(x => x.Permission.Rights == "Read" && x.Level == level && x.Menu.MenuId == menuId)
                .SelectMany(x => x.Menu.Children).ToListAsync();
        }
    }
}
