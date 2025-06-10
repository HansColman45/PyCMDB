using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.RolePermission;

namespace CMDB.UI.Specflow.Questions.RolePermissions
{
    public class OpenTheEditRolePermissionPage : Question<EditRolePermisionPage>
    {
        public override EditRolePermisionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<RolePermissionOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            EditRolePermisionPage editRolePermisionPage = WebPageFactory.Create<EditRolePermisionPage>(page.WebDriver);
            return editRolePermisionPage;
        }
    }
}
