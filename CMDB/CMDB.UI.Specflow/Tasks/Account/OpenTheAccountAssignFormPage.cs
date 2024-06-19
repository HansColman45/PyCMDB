using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks.Account
{
    public class OpenTheAccountAssignFormPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountAssignIdentityPage>();
            page.ClickElementByXpath("//button[@type='submit']");
        }
    }
}
