using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Token;

namespace CMDB.UI.Specflow.Questions.Token
{
    public class OpenTheTokenCreatePage : Question<CreateTokenPage>
    {
        public override CreateTokenPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheTokenDetailPage : Question<TokenDetailPage>
    {
        public override TokenDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheTokenEditPage : Question<UpdateTokenPage>
    {
        public override UpdateTokenPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheTokenDeactivatePage : Question<DeactivateTokenPage>
    {
        public override DeactivateTokenPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheTokenAssignIdentityPage : Question<TokenAssignIdentityPage>
    {
        public override TokenAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(page.WebDriver);
        }
    }
}
