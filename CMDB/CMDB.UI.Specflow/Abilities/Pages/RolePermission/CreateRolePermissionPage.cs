using CMDB.Domain.Entities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.RolePermission
{
    public class CreateRolePermissionPage: MainPage
    {
        public CreateRolePermissionPage(IWebDriver web) : base(web)
        {
        }
        public void SelectMenu(Menu menu)
        {
            SelectValueInDropDownByXpath("//select[@id='Menu']", $"{menu.MenuId}");
        }
        public void SelectPermission(Permission permission)
        {
            SelectValueInDropDownByXpath("//select[@id='Permission']", $"{permission.Id}");
        }
        public void SelectLevel(string level)
        {
            SelectValueInDropDownByXpath("//select[@id='Level']", level);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
