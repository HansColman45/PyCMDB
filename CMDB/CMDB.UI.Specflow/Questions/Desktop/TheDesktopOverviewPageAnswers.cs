using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;

namespace CMDB.UI.Specflow.Questions.Desktop
{
    public class OpenTheDesktopCreatePage : Question<CreateDesktopPage>
    {
        public override CreateDesktopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheDesktopDetailPage : Question<DesktopDetailPage>
    {
        public override DesktopDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheDesktopEditPage : Question<UpdateDesktopPage>
    {
        public override UpdateDesktopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheDesktopDeactivatePage : Question<DeactivateDesktopPage>
    {
        public override DeactivateDesktopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheDesktopAssignIdentityPage : Question<DesktopAssignIdentityPage>
    {
        public override DesktopAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
            return new(page.WebDriver);
        }
    }
}
