namespace CMDB.UI.Specflow.Abilities.Pages.Docking
{
    public class DockingAssignIdentityPage : MainPage
    {
        public DockingAssignIdentityPage() : base()
        {
        }
        public void SelectIdentity(Domain.Entities.Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", $"{identity.IdenId}");
        }
    }
}
