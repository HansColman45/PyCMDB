using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AccountPages
{
    public class AccountOverviewPage : MainPage
    {
        public AccountOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
