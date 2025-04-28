using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    public class OpenTheLaptopAssignIdentityPage : Question<LaptopAssignIdentityPage>
    {
        public override LaptopAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            LaptopAssignIdentityPage laptopAssignIdentityPage = WebPageFactory.Create<LaptopAssignIdentityPage>(page.WebDriver);
            return laptopAssignIdentityPage;
        }
    }
}
