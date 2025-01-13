using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingEditPage : Question<UpdateDockingPage>
    {
        public override UpdateDockingPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            return new();
        }
    }
}
