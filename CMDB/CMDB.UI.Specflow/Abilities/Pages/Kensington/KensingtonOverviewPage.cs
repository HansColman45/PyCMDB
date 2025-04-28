using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Kensington
{
    public class KensingtonOverviewPage: MainPage
    {
        public KensingtonOverviewPage(IWebDriver web) : base(web)
        {   
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
