using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using BrightTasks = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks
{
    public class ClickTheGeneratePDFOnReleaseForm : BrightTasks
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<ReleaseDevicePage>();
            page.ClickElementByXpath("//button[@type='submit']");
            page.WaitOnAddNew();
        }
    }
}
