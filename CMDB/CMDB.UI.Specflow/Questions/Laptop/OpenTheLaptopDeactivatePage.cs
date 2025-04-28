using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    public class OpenTheLaptopDeactivatePage : Question<DeactivateLaptopPage>
    {
        public override DeactivateLaptopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(MainPage.DeactivateXpath);
            DeactivateLaptopPage deactivateLaptopPage = WebPageFactory.Create<DeactivateLaptopPage>(page.WebDriver);
            return deactivateLaptopPage;
        }
    }
}
