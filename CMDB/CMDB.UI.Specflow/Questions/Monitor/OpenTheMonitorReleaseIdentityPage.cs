using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Monitor
{
    public class OpenTheMonitorReleaseIdentityPage : Question<MonitorReleaseIdentityPage>
    {
        public override MonitorReleaseIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MonitorDetailPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.ReleaseIdenityXpath);
            return new();
        }
    }
}
