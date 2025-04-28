using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Token;

namespace CMDB.UI.Specflow.Questions.Token
{
    public class OpenTheTokenEditPage : Question<UpdateTokenPage>
    {
        public override UpdateTokenPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TokenOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            UpdateTokenPage updateTokenPage = WebPageFactory.Create<UpdateTokenPage>(page.WebDriver);
            return updateTokenPage;
        }
    }
}
