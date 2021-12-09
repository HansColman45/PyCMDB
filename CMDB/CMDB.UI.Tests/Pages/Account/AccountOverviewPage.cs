using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class AccountOverviewPage : MainPage
    {
        public AccountOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateAccountPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public AccountDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public EditAccountPage Edit()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateAccountPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
