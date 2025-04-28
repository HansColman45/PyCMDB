using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Docking
{
    public class DockingOverviewPage : MainPage
    {
        public DockingOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
