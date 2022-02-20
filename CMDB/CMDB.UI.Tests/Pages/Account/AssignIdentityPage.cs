using CMDB.Domain.Entities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class AssignIdentityPage: MainPage
    {
        public AssignIdentityPage(IWebDriver driver):base(driver)
        {   
        }
        public void SelectIdentity(Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", identity.IdenId.ToString());
        }
        public string ValidFrom
        {
            set => EnterInTextboxByXPath("//input[@id='ValidFrom']", value);
        }
        public string ValidUntil
        {
            set => EnterInTextboxByXPath("//input[@id='ValidUntil']", value);
        }
        public AssignFormPage Assign()
        {
            ClickElementByXpath("//button[@type='submit']");
            return new(driver);
        }
    }
}
