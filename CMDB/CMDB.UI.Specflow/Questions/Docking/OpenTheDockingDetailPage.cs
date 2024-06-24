using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingDetailPage : Question<DockingOverviewPage>
    {
        public override DockingOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            return new();
        }
    }
}
