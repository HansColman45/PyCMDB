using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Monitor
{
    public class MonitorAssignIdentityPage : MainPage
    {
        public MonitorAssignIdentityPage() : base()
        {
        }
        public void SelectIdentity(Domain.Entities.Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", $"{identity.IdenId}");
        }
        public AssignFormPage Assign()
        {
            ClickElementByXpath("//button[.='Assign']");
            return new();
        }
    }
}
