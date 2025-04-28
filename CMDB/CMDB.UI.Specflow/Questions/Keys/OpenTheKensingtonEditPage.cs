using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;

namespace CMDB.UI.Specflow.Questions.Keys
{
    public class OpenTheKensingtonEditPage : Question<KensingtonEditPage>
    {
        public override KensingtonEditPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<KensingtonOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            KensingtonEditPage kensingtonEditPage = WebPageFactory.Create<KensingtonEditPage>(page.WebDriver);
            return kensingtonEditPage;
        }
    }
}
