using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AccountPages
{
    public class AccountOverviewPage : MainPage
    {
        public AccountOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateAccountPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(WebDriver);
        }
        public AccountDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(WebDriver);
        }
        public EditAccountPage Edit()
        {
            ClickElementByXpath(EditXpath);
            return new(WebDriver);
        }
        public DeactivateAccountPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(WebDriver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public AccountAssignIdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(WebDriver);
        }
    }
}
