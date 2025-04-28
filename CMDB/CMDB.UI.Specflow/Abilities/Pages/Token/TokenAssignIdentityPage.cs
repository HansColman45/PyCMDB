using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Token
{
    public class TokenAssignIdentityPage : MainPage
    {
        public TokenAssignIdentityPage(IWebDriver web) : base(web)
        {
        }
        public void SelectIdentity(Domain.Entities.Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", $"{identity.IdenId}");
        }
    }
}
