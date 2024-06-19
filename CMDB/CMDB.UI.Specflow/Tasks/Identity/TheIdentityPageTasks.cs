using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks.Identity
{
    public class OpenTheCreateIdentityPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
        }
    }
    public class OpenTheIdentityDetailPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
        }
    }
    public class OpenTheUpdateIdentityPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            page.WaitUntilElmentVisableByXpath("//input[@name='FirstName']");
        }
    }
    public class OpenTheDeactivateIdentityPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            page.WaitUntilElmentVisableByXpath("//input[@id='reason']");
        }
    }
    public class OpenTheAssignAccountPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath("//a[@title='Assign Account']");
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
        }
    }
    public class OpenTheAssignDevicePage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath("//a[@title='Assign Device']");
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
        }
    }
}
