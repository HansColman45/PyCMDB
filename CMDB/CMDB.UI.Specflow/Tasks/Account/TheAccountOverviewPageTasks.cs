using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks.Account
{
    public class OpenTheAccountCreatePage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
        }
    }
    public class OpenTheAccountDetailPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
        }
    }
    public class OpenTheAccountEditPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
        }
    }
    public class OpenTheAccountDeactivatePage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
        }
    }
    public class OpenTheAccountAssignIdentityPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
        }
    }
}
