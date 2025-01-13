using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Desktop
{
    public class OpenTheDesktopAssignIdentityPage : Question<DesktopAssignIdentityPage>
    {
        public override DesktopAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
            return new();
        }
    }
}
