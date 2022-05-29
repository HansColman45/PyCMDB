using CMDB.Domain.Entities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class AssignToken2IdentityPage : MainPage
    {
        public AssignToken2IdentityPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public void SelectIdentity(Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", $"{identity.IdenId}");
        }
        public AssignFormPage Assign()
        {
            ClickElementByXpath("//button[.='Assign']");
            return new(driver);
        }
    }
}
