using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Desktop
{
    public class DesktopAssignIdentityPage : MainPage
    {
        public DesktopAssignIdentityPage(IWebDriver web) : base(web)
        {
        }
        public void SelectIdentity(Domain.Entities.Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", $"{identity.IdenId}");
        }
    }
}
