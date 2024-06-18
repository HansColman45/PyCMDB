using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Identity;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheCreateIdentityPage : Question<CreateIdentityPage>
    {
        public override CreateIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheIdentityDetailPage : Question<IdentityDetailPage>
    {
        public override IdentityDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheUpdateIdentityPage : Question<UpdateIdentityPage>
    {
        public override UpdateIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            page.WaitUntilElmentVisableByXpath("//input[@name='FirstName']");
            return new(page.WebDriver);
        }
    }
    public class OpenTheDeactivateIdentityPage : Question<DeactivateIdentityPage>
    {
        public override DeactivateIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            page.WaitUntilElmentVisableByXpath("//input[@id='reason']");
            return new(page.WebDriver);
        }
    }
    public class OpenTheAssignAccountPage : Question<AssignAccountPage>
    {
        public override AssignAccountPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath("//a[@title='Assign Account']");
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(page.WebDriver);
        }
    }
    public class OpenTheAssignDevicePage : Question<AssignDevicePage>
    {
        public override AssignDevicePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath("//a[@title='Assign Device']");
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(page.WebDriver);
        }
    }
}
