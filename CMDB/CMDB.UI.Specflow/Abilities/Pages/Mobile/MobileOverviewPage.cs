using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Mobile
{
    public class MobileOverviewPage : MainPage
    {
        public MobileOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
