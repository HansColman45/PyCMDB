using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks.Docking
{
    public class OpenTheDockingCreatePage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
        }
    }
    public class OpenTheDockingDetailPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
        }
    }
    public class OpenTheDockingEditPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
        }
    }
    public class OpenTheDockingDeactivatePage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
        }
    }
    public class OpenTheDockingAssignIdentityPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
        }
    }
}
