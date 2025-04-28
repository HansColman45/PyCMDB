using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    public class OpenTheMobileDeactivatePage : Question<DeactivateMobilePage>
    {
        public override DeactivateMobilePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            DeactivateMobilePage deactivateMobilePage = WebPageFactory.Create<DeactivateMobilePage>(page.WebDriver);
            return deactivateMobilePage;
        }
    }
}
