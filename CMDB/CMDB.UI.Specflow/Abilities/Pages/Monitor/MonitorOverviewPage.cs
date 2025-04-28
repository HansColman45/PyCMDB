using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Monitor
{
    public class MonitorOverviewPage : MainPage
    {
        public MonitorOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
