using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;

namespace CMDB.UI.Specflow.Questions.Laptop
{
    public class OpenTheLaptopCreatePage : Question<CreateLaptopPage>
    {
        public override CreateLaptopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheLaptopDetailPage : Question<LaptopDetailPage>
    {
        public override LaptopDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheLaptopEditPage : Question<UpdateLaptopPage>
    {
        public override UpdateLaptopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheLaptopDeactivatePage : Question<DeactivateLaptopPage>
    {
        public override DeactivateLaptopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheLaptopAssignIdentityPage : Question<LaptopAssignIdentityPage>
    {
        public override LaptopAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LaptopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(page.WebDriver);
        }
    }
}
