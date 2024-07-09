namespace CMDB.UI.Specflow.Abilities.Pages.Token
{
    public class TokenAssignIdentityPage : MainPage
    {
        public TokenAssignIdentityPage() : base()
        {
        }
        public void SelectIdentity(Domain.Entities.Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", $"{identity.IdenId}");
        }
    }
}
