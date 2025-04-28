using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    public class OpenTheMobileEditPage : Question<UpdateMobilePage>
    {
        public override UpdateMobilePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            UpdateMobilePage updateMobilePage = WebPageFactory.Create<UpdateMobilePage>(page.WebDriver);
            return updateMobilePage;
        }
    }
}
