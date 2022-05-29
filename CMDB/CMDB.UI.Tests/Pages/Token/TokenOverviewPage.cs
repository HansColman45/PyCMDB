using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class TokenOverviewPage : MainPage
    {
        public TokenOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateTokenPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public TokenDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public UpdateTokenPage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateTokenPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public AssignToken2IdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(driver);
        }
    }
}
