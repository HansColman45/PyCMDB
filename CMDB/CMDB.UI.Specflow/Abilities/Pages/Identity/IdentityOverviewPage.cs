using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class IdentityOverviewPage : MainPage
    {
        public IdentityOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateIdentityPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(WebDriver);
        }
        public IdentityDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(WebDriver);
        }
        public UpdateIdentityPage Update()
        {
            ClickElementByXpath(EditXpath);
            WaitUntilElmentVisableByXpath("//input[@name='FirstName']");
            return new(WebDriver);
        }
        public DeactivateIdentityPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            WaitUntilElmentVisableByXpath("//input[@id='reason']");
            return new(WebDriver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public AssignAccountPage AssignAccount()
        {
            ClickElementByXpath("//a[@title='Assign Account']");
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(WebDriver);
        }
        public AssignDevicePage AssignDevice()
        {
            ClickElementByXpath("//a[@title='Assign Device']");
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(WebDriver);
        }
    }
}