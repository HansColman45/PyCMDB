using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;

namespace CMDB.UI.Specflow.Questions.Keys
{
    public class OpenTheKensingtonDeactivatePage : Question<KensingtonDeactivatePage>
    {
        public override KensingtonDeactivatePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<KensingtonOverviewPage>();
            page.ClickElementByXpath(MainPage.DeactivateXpath);
            page.WaitUntilElmentVisableByXpath("//input[@id='reason']");
            KensingtonDeactivatePage kensingtonDeactivatePage = WebPageFactory.Create<KensingtonDeactivatePage>(page.WebDriver);
            return kensingtonDeactivatePage;
        }
    }
}
