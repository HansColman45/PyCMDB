using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheKensingtonOverviewPage : Question<KensingtonOverviewPage>
    {
        public override KensingtonOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Kensington21']");
            page.ClickElementByXpath("//a[@href='/Kensington']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
