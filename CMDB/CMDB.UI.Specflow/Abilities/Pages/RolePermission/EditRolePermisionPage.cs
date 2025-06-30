using CMDB.Domain.Entities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.RolePermission
{
    public class EditRolePermisionPage: MainPage
    {
        public EditRolePermisionPage(IWebDriver web) : base(web)
        {
        }
        public void SelectLevel(string level)
        {
            SelectValueInDropDownByXpath("//select[@id='Level']", level);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}
