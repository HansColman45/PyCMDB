using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Account
{
    public class OpenTheAccountDeactivatePage : Question<OpenTheAccountDeactivatePage>
    {
        public override OpenTheAccountDeactivatePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            return new();
        }
    }
}
