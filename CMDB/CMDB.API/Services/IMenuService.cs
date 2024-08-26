using CMDB.Domain.Entities;

namespace CMDB.API.Services
{
    public interface IMenuService
    {
        Task<ICollection<Menu>> ListFirstMenuLevel();
        Task<ICollection<Menu>> ListSecondMenuLevel(int menuID);
        Task<ICollection<Menu>> ListPersonalMenu(int level, int menuID);
    }
}
