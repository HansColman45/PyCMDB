using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.AssetTypes
{
    public class AssetTypeOverviewPage : MainPage
    {
        public AssetTypeOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateAssetTypePage New()
        {
            ClickElementByXpath(NewXpath);
            return new(WebDriver);
        }
        public AssetTypeDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(WebDriver);
        }
        public UpdateAssetTypePage Edit()
        {
            ClickElementByXpath(EditXpath);
            return new(WebDriver);
        }
        public DeactivateAssetTypePage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(WebDriver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
