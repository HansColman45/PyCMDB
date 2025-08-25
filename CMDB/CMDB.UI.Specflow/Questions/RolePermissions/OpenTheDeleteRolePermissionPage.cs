using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.RolePermission;

namespace CMDB.UI.Specflow.Questions.RolePermissions
{
    public class OpenTheDeleteRolePermissionPage : Question<DeleteRolePermissionPage>
    {
        public override DeleteRolePermissionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<RolePermissionOverviewPage>();
            page.ClickElementByXpath("//a[@title='Delete']");
            DeleteRolePermissionPage deleteRolePermissionPage = WebPageFactory.Create<DeleteRolePermissionPage>(page.WebDriver);
            return deleteRolePermissionPage;
        }
    }
}
