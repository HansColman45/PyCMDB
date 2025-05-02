using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Docking;

namespace CMDB.UI.Specflow.Questions.Docking
{
    internal class OpenTheDockingReleaseKensingtonPage : Question<DockingReleaseKensingtonPage>
    {
        public override DockingReleaseKensingtonPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(MainPage.ReleaseKensingtonXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            DockingReleaseKensingtonPage dockingReleaseKensingtonPage = WebPageFactory.Create<DockingReleaseKensingtonPage>(page.WebDriver);
            return dockingReleaseKensingtonPage;
        }
    }
}
