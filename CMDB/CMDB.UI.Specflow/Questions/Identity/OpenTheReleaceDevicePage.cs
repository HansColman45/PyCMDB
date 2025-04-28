using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheReleaceDevicePage : Question<ReleaseDevicePage>
    {
        public override ReleaseDevicePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityDetailPage>();
            page.ClickElementByXpath(MainPage.ReleaseDeviceXPath);
            ReleaseDevicePage releaseDevicePage = WebPageFactory.Create<ReleaseDevicePage>(page.WebDriver);
            return releaseDevicePage;
        }
    }
}
