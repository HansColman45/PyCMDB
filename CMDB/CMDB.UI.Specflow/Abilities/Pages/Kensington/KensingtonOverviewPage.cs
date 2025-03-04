using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Kensington
{
    public class KensingtonOverviewPage: MainPage
    {
        public KensingtonOverviewPage() : base()
        {   
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
