using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheDesktopOverviewPage : Question<DesktopOverviewPage>
    {
        public override DesktopOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Desktop13']");
            page.ClickElementByXpath("//a[@href='/Desktop']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
