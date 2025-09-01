using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Permissions;

namespace CMDB.UI.Specflow.Questions.Permissions
{
    public class OpenThePermissionDetailPage : Question<PermissionDetailPage>
    {
        public override PermissionDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<PermissionOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            PermissionDetailPage detailPage = WebPageFactory.Create<PermissionDetailPage>(page.WebDriver);
            return detailPage;
        }
    }
}
