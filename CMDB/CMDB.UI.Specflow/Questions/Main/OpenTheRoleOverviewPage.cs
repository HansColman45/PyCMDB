using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheRoleOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Role']");
            page.ClickElementByXpath("//a[@id='Role']");
            page.ClickElementByXpath("//a[@id='Role8']");
            page.ClickElementByXpath("//a[@href='/Role']");
            page.WaitOnAddNew();
        }
    }
}
