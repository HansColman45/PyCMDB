using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.RolePermission;

namespace CMDB.UI.Specflow.Questions.RolePermissions
{
    public class OpenTheRolePermissionDetailPage : Question<RolePermissionDetailPage>
    {
        public override RolePermissionDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<RolePermissionOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            RolePermissionDetailPage rolePermissionDetailPage = WebPageFactory.Create<RolePermissionDetailPage>(page.WebDriver);
            return rolePermissionDetailPage;
        }
    }
}
