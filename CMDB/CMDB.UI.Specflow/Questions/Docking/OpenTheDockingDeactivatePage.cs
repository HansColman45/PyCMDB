using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingDeactivatePage : Question<DeactivateDockingPage>
    {
        public override DeactivateDockingPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            DeactivateDockingPage deactivateDockingPage = WebPageFactory.Create<DeactivateDockingPage>(page.WebDriver);
            return deactivateDockingPage;
        }
    }
}
