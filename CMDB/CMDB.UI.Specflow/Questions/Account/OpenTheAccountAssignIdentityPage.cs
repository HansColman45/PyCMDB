using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Account
{
    public class OpenTheAccountAssignIdentityPage : Question<AccountAssignIdentityPage>
    {
        public override AccountAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
            return new();
        }
    }
}
