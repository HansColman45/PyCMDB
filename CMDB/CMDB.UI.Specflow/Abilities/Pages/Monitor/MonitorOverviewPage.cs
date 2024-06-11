using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Monitor
{
    public class MonitorOverviewPage : MainPage
    {
        public MonitorOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateMonitorPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(WebDriver);
        }
        public MonitorDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(WebDriver);
        }
        public UpdateMonitorPage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(WebDriver);
        }
        public DeactivateMonitorPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(WebDriver);
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
            return new(WebDriver);
        }
    }
}
