using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Docking
{
    public class OpenTheDockingCreatePage : Question<CreateDockingPage>
    {
        public override CreateDockingPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DockingOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            CreateDockingPage createDockingPage = WebPageFactory.Create<CreateDockingPage>(page.WebDriver);
            return createDockingPage;
        }
    }
}
