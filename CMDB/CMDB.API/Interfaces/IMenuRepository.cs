using CMDB.Domain.Entities;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// The interface for menu
    /// </summary>
    public interface IMenuRepository
    {
        /// <summary>
        /// Retrieves all menu items asynchronously.
        /// </summary>
        /// <returns>A list of <see cref="Menu"/></returns>
        Task<IEnumerable<Menu>> GetAll();
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
        /// <summary>
        /// This will return the details of a Menu by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Menu> GetById(int id);
    }
}
