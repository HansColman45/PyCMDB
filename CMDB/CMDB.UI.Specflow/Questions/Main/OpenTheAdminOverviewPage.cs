using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Admin;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheAdminOverviewPage : Question<AdminOverviewPage>
    {
        public override AdminOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin47']");
            page.ClickElementByXpath("//a[@href='/Admin']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
