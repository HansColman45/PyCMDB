using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Keys
{
    class OpenTheKensingtonCreatePage: Question<KensingtonCreatePage>
    {
        public override KensingtonCreatePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<KensingtonOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            KensingtonCreatePage kensingtonCreatePage = WebPageFactory.Create<KensingtonCreatePage>(page.WebDriver);
            return kensingtonCreatePage;
        }
    }
}
