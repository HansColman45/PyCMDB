using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheUpdateIdentityPage : Question<UpdateIdentityPage>
    {
        public override UpdateIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(IdentityOverviewPage.EditXpath);
            page.WaitUntilElmentVisableByXpath("//input[@name='FirstName']");
            return new();
        }
    }
}
