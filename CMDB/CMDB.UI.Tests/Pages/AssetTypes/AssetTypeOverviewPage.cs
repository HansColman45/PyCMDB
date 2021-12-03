using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class AssetTypeOverviewPage : MainPage
    {
        public AssetTypeOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateAssetTypePage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public AssetTypeDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public AssetTypeEditPage Edit()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateAssetTypePage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
