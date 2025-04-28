using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Token;

namespace CMDB.UI.Specflow.Questions.Token
{
    public class OpenTheTokenCreatePage : Question<CreateTokenPage>
    {
        public override CreateTokenPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreateTokenPage createTokenPage = WebPageFactory.Create<CreateTokenPage>(page.WebDriver);
            return createTokenPage;
        }
    }
}
