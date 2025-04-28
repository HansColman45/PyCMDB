using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    public class OpenTheMobileReleaseIdentityPage : Question<MobileReleaseIdentityPage>
    {
        public override MobileReleaseIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileDetailPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.ReleaseIdenityXpath);
            MobileReleaseIdentityPage mobileReleaseIdentityPage = WebPageFactory.Create<MobileReleaseIdentityPage>(page.WebDriver);
            return mobileReleaseIdentityPage;
        }
    }
}
