using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class DockingOverviewPage : MainPage
    {
        public DockingOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateDockingPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public DockingDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public UpdateDockingPage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateDockingPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public DockingAssignIdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(driver);
        }
    }
}
