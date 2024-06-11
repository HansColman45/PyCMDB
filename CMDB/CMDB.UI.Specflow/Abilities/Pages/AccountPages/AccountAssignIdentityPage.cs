using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AccountPages
{
    public class AccountAssignIdentityPage : MainPage
    {
        public AccountAssignIdentityPage(IWebDriver driver) : base(driver)
        {
        }
        public void SelectIdentity(Domain.Entities.Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", identity.IdenId.ToString());
        }
        public DateTime ValidFrom
        {
            set => EnterDateTimeByXPath("//input[@id='ValidFrom']", value);
        }
        public DateTime ValidUntil
        {
            set => EnterDateTimeByXPath("//input[@id='ValidUntil']", value);
        }
        public AssignFormPage Assign()
        {
            ClickElementByXpath("//button[@type='submit']");
            return new(WebDriver);
        }
    }
}
