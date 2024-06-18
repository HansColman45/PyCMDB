using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Types;

namespace CMDB.UI.Specflow.Questions.Types
{
    public class OpenTheTypeCreatePage : Question<CreateTypePage>
    {
        public override CreateTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(TypeOverviewPage.NewXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheTypeDetailsPage : Question<TypeDetailPage>
    {
        public override TypeDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(TypeOverviewPage.InfoXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheTypeEditPage : Question<UpdateTypePage>
    {
        public override UpdateTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(TypeOverviewPage.EditXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheTypeDeactivatePage : Question<DeactivateTypePage>
    {
        public override DeactivateTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(TypeOverviewPage.DeactivateXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheTypeAssignIdentityPage : Question<TypeAssignIdentityPage>
    {
        public override TypeAssignIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(TypeOverviewPage.AssignIdenityXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(page.WebDriver);
        }
    }
}
