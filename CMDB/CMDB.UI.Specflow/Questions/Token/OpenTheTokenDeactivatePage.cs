using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Token;

namespace CMDB.UI.Specflow.Questions.Token
{
    public class OpenTheTokenDeactivatePage : Question<DeactivateTokenPage>
    {
        public override DeactivateTokenPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            DeactivateTokenPage deactivateTokenPage = WebPageFactory.Create<DeactivateTokenPage>(page.WebDriver);
            return deactivateTokenPage;
        }
    }
}
