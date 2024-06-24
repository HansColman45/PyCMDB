using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheMobileOverviewPage : Question<MobileOverviewPage>
    {
        public override MobileOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Mobile23']");
            page.ClickElementByXpath("//a[@href='/Mobile']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
