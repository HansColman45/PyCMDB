using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    public class OpenTheLaptopReleaseIdentityPage : Question<LaptopReleaseIdentityPage>
    {
        public override LaptopReleaseIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopDetailPage>();
            page.ClickElementByXpath(MainPage.ReleaseIdenityXpath);
            LaptopReleaseIdentityPage laptopReleaseIdentityPage = WebPageFactory.Create<LaptopReleaseIdentityPage>(page.WebDriver);
            return laptopReleaseIdentityPage;
        }
    }
}
