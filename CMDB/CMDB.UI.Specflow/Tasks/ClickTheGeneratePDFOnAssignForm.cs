using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using BrightTasks = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks
{
    public class ClickTheGeneratePDFOnAssignForm : BrightTasks
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AssignFormPage>();
            page.ClickElementByXpath("//button[@type='submit']");
            page.WaitOnAddNew();
        }
    }
}
