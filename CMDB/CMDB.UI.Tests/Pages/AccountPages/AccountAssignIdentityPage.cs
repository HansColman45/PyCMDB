using CMDB.Domain.Entities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class AccountAssignIdentityPage: MainPage
    {
        public AccountAssignIdentityPage(IWebDriver driver):base(driver)
        {   
        }
        public void SelectIdentity(Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", identity.IdenId.ToString());
        }
        public DateTime ValidFrom
        {
            set => EnterDateTimeByXPath("//input[@id='ValidFrom']", value);
        }
        public DateTime ValidUntil
        {
            set => EnterDateTimeByXPath("//input[@id='ValidUntil']", value);
        }
        public AssignFormPage Assign()
        {
            ClickElementByXpath("//button[@type='submit']");
            return new(driver);
        }
    }
}
