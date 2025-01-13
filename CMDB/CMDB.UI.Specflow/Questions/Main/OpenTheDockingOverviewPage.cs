using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Docking;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheDockingOverviewPage : Question<DockingOverviewPage>
    {
        public override DockingOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Docking station17']");
            page.ClickElementByXpath("//a[@href='/Docking']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
