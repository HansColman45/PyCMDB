using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheLaptopOverviewPage : Question<LaptopOverviewPage>
    {
        public override LaptopOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Laptop11']");
            page.ClickElementByXpath("//a[@href='/Laptop']");
            page.WaitOnAddNew();
            LaptopOverviewPage laptopOverviewPage = WebPageFactory.Create<LaptopOverviewPage>(page.WebDriver);
            return laptopOverviewPage;
        }
    }
}
