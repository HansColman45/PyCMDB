using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AssetTypes
{
    public class AssetTypeOverviewPage : MainPage
    {
        public AssetTypeOverviewPage(IWebDriver web) : base(web)
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
