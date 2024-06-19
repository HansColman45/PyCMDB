using Bright.ScreenPlay.Actors;
using Task = Bright.ScreenPlay.Tasks.Task;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;

namespace CMDB.UI.Specflow.Questions.SubscriptionType
{
    public class  OpenTheSubscriptionTypeCreatePage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
        }
    }
    public class  OpenTheSubscriptionTypeDetailPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
        }
    }
    public class  OpenTheSubscriptionTypeEditPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
        }
    }
    public class  OpenTheSubscriptionTypeDeactivatePage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
        }
    }
}
