using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Admin;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenThePermissionOverviewPage : Question<PermissionOverviewPage>
    {
        public override PermissionOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Permissions49']");
            page.ClickElementByXpath("//a[@href='/Permission']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
