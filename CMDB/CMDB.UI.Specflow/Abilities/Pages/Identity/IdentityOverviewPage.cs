using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class IdentityOverviewPage : MainPage
    {
        public IdentityOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}