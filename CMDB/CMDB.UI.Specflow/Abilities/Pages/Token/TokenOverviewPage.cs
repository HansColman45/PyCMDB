using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Token
{
    public class TokenOverviewPage : MainPage
    {
        public TokenOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateTokenPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(WebDriver);
        }
        public TokenDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(WebDriver);
        }
        public UpdateTokenPage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(WebDriver);
        }
        public DeactivateTokenPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(WebDriver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public TokenAssignIdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(WebDriver);
        }
    }
}
