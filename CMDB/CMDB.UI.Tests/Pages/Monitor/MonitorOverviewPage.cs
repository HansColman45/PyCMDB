using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class MonitorOverviewPage : MainPage
    {
        public MonitorOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateMonitorPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public MonitorDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public UpdateMonitorPage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateMonitorPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public MonitorAssignIdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(driver);
        }
    }
}
