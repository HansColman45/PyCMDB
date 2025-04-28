using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    internal class OpenTheReleaseIdentityPage : Question<LaptopReleaseIdentityPage>
    {
        public override LaptopReleaseIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityDetailPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.ReleaseIdenityXpath);
            LaptopReleaseIdentityPage laptopReleaseIdentityPage = WebPageFactory.Create<LaptopReleaseIdentityPage>(page.WebDriver);
            return laptopReleaseIdentityPage;
        }
    }
}
