using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    public class OpenTheMobileCreatePage : Question<CreateMobilePage>
    {
        public override CreateMobilePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheMobileDetailPage : Question<MobileDetailPage>
    {
        public override MobileDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheMobileEditPage : Question<UpdateMobilePage>
    {
        public override UpdateMobilePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheMobileDeactivatePage : Question<DeactivateMobilePage>
    {
        public override DeactivateMobilePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheMobileAssignIdentityPage : Question<MobileAssignIdentityPage>
    {
        public override MobileAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(page.WebDriver);
        }
    }
}
