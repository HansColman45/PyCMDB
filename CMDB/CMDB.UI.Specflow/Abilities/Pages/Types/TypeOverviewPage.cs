using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Types
{
    public class TypeOverviewPage : MainPage
    {
        public TypeOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
