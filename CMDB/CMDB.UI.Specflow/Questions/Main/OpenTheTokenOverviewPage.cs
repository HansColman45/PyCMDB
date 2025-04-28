using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Token;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheTokenOverviewPage : Question<TokenOverviewPage>
    {
        public override TokenOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Token19']");
            page.ClickElementByXpath("//a[@href='/Token']");
            page.WaitOnAddNew();
            TokenOverviewPage tokenOverviewPage = WebPageFactory.Create<TokenOverviewPage>(page.WebDriver);
            return tokenOverviewPage;
        }
    }
}
