using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Account
{
    public class OpenTheAccountReleaseIdentityPage : Question<AccountReleaseIdentityPage>
    {
        public override AccountReleaseIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountDetailPage>();
            page.ClickElementByXpath("//a[@id='ReleaseIdentity']");
            AccountReleaseIdentityPage accountReleaseIdentity = WebPageFactory.Create<AccountReleaseIdentityPage>(page.WebDriver);
            return accountReleaseIdentity;
        }
    }
}
