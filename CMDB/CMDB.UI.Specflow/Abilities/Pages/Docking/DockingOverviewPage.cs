using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Docking
{
    public class DockingOverviewPage : MainPage
    {
        public DockingOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateDockingPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(WebDriver);
        }
        public DockingDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(WebDriver);
        }
        public UpdateDockingPage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(WebDriver);
        }
        public DeactivateDockingPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(WebDriver);
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
            return new(WebDriver);
        }
    }
}
