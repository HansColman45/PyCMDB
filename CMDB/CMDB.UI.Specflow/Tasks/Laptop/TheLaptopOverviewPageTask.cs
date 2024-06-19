using Bright.ScreenPlay.Actors;
using Task = Bright.ScreenPlay.Tasks.Task;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Tasks.Laptop
{
    public class OpenTheLaptopCreatePage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
        }
    }
    public class OpenTheLaptopDetailPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
        }
    }
    public class OpenTheLaptopEditPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
        }
    }
    public class OpenTheLaptopDeactivatePage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
        }
    }
    public class OpenTheLaptopAssignIdentityPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
        }
    }
}
