using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.RolePermission;

namespace CMDB.UI.Specflow.Questions.RolePermissions
{
    public class OpenTheCreateRolePermissionPage : Question<CreateRolePermissionPage>
    {
        public override CreateRolePermissionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<RolePermissionOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreateRolePermissionPage createRolePermissionPage = WebPageFactory.Create<CreateRolePermissionPage>(page.WebDriver);
            return createRolePermissionPage;
        }
    }
}
