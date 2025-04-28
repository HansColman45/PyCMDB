using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Desktop
{
    public class DesktopOverviewPage : MainPage
    {
        public DesktopOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
