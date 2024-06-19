using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks
{
    public class OpenTheMainPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LoginPage>();
            page.ClickElementByXpath("//button[@type='submit']");
            page.WaitUntilElmentVisableByXpath("//h1");
        }
    }
}
