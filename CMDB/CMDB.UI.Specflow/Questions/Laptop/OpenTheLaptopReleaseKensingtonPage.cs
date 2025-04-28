using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    public class OpenTheLaptopReleaseKensingtonPage : Question<LaptopReleaseKensingtonPage>
    {
        public override LaptopReleaseKensingtonPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopDetailPage>();
            page.ClickElementByXpath(MainPage.ReleaseKensingtonXpath);
            LaptopReleaseKensingtonPage laptopReleaseKensingtonPage = WebPageFactory.Create<LaptopReleaseKensingtonPage>(page.WebDriver);
            return laptopReleaseKensingtonPage;
        }
    }
}
