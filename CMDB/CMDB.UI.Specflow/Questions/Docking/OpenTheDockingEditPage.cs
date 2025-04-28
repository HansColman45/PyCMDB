using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingEditPage : Question<UpdateDockingPage>
    {
        public override UpdateDockingPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            UpdateDockingPage updateDockingPage = WebPageFactory.Create<UpdateDockingPage>(page.WebDriver);
            return updateDockingPage;
        }
    }
}
