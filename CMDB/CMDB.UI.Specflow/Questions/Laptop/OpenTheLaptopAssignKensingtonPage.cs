using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    public class OpenTheLaptopAssignKensingtonPage : Question<LaptopAssignKensingtonPage>
    {
        public override LaptopAssignKensingtonPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(MainPage.AssignKensingtonXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            LaptopAssignKensingtonPage laptopAssignKensingtonPage = WebPageFactory.Create<LaptopAssignKensingtonPage>(page.WebDriver);
            return laptopAssignKensingtonPage;
        }
    }
}
