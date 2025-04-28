using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Docking;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingDetailPage : Question<DockingDetailPage>
    {
        public override DockingDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            DockingDetailPage dockingDetailPage = WebPageFactory.Create<DockingDetailPage>(page.WebDriver);
            return dockingDetailPage;
        }
    }
}
