using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Mobile
{
    public class MobileAssignIdentityPage : MainPage
    {
        public MobileAssignIdentityPage() : base()
        {
        }
        public void SelectIdentity(Domain.Entities.Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", $"{identity.IdenId}");
        }
    }
}
