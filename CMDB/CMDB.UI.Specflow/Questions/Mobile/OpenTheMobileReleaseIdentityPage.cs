using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    internal class OpenTheMobileReleaseIdentityPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityDetailPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.ReleaseIdenityXpath);
        }
    }
}
