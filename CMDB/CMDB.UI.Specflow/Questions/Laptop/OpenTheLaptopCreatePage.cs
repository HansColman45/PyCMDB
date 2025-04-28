using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    public class OpenTheLaptopCreatePage : Question<CreateLaptopPage>
    {
        public override CreateLaptopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            CreateLaptopPage createLaptopPage = WebPageFactory.Create<CreateLaptopPage>(page.WebDriver);
            return createLaptopPage;
        }
    }
}