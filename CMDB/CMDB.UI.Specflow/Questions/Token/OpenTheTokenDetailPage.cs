using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Token;

namespace CMDB.UI.Specflow.Questions.Token
{
    public class OpenTheTokenDetailPage : Question<TokenDetailPage>
    {
        public override TokenDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            TokenDetailPage tokenDetailPage = WebPageFactory.Create<TokenDetailPage>(page.WebDriver);
            return tokenDetailPage;
        }
    }
}
