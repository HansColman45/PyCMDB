using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Laptop
{
    public class LaptopOverviewPage : MainPage
    {
        public LaptopOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
