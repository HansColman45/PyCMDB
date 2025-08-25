using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.RolePermission
{
    public class DeleteRolePermissionPage : MainPage
    {
        public DeleteRolePermissionPage(IWebDriver driver): base(driver)
        {
        }

        public void Delete()
        {
            ClickElementByXpath("//button[.='Delete']");
        }
    }
}
