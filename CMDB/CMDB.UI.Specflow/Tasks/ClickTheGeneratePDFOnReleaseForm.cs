using Bright.ScreenPlay.Actors;
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
    public class ClickTheGeneratePDFOnReleaseMobileForm : BrightTasks
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<ReleaseMobilePage>();
            page.ClickElementByXpath("//button[@type='submit']");
            page.WaitOnAddNew();
        }
    }
    public class ClickTheGeneratePDFOnReleaseSubscriptionForm : BrightTasks
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<ReleaseSubscriptionPage>();
            page.ClickElementByXpath("//button[@type='submit']");
            page.WaitOnAddNew();
        }
    }
}
