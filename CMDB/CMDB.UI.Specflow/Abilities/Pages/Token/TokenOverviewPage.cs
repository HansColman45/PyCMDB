using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Token
{
    public class TokenOverviewPage : MainPage
    {
        public TokenOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
