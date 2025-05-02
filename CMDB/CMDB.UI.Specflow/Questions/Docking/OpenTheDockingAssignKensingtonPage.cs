using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Docking;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingAssignKensingtonPage: Question<DockingAssignKensingtonPage>
    {
        public override DockingAssignKensingtonPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(MainPage.AssignKensingtonXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            DockingAssignKensingtonPage dockingAssignKensingtonPage = WebPageFactory.Create<DockingAssignKensingtonPage>(page.WebDriver);
            return dockingAssignKensingtonPage;
        }
    }
}
