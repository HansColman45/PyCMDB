using CMDB.Domain.Entities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.RolePermission
{
    public class EditRolePermisionPage: MainPage
    {
        public EditRolePermisionPage(IWebDriver web) : base(web)
        {
        }
        public void SelectMenu(Menu menu)
        {
            SelectValueInDropDownByXpath("//select[@id='Menu']", $"{menu.MenuId}");
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}
