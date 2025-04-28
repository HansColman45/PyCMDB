using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    public class OpenTheLaptopEditPage : Question<UpdateLaptopPage>
    {
        public override UpdateLaptopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            UpdateLaptopPage updateLaptopPage = WebPageFactory.Create<UpdateLaptopPage>(page.WebDriver);
            return updateLaptopPage;
        }
    }
}
