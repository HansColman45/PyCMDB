using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class DesktopOverviewPage : MainPage
    {
        public DesktopOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateDesktopPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public DesktopDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public UpdateDesktopPage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateDesktopPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public DesktopAssignIdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(driver);
        }
    }
}
