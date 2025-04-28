using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;

namespace CMDB.UI.Specflow.Questions.Keys
{
    class OpenTheKensingtonDetailPage : Question<KensingtonDetailPage>
    {
        public override KensingtonDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<KensingtonOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            KensingtonDetailPage kensingtonDetailPage = WebPageFactory.Create<KensingtonDetailPage>(page.WebDriver);
            return kensingtonDetailPage;
        }
    }
}
