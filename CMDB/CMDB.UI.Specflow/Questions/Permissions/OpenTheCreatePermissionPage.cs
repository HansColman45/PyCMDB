using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Permissions;
using CMDB.UI.Specflow.Abilities.Pages.RolePermission;

namespace CMDB.UI.Specflow.Questions.Permissions
{
    public class OpenTheCreatePermissionPage : Question<CreatePermissionPage>
    {
        public override CreatePermissionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<PermissionOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreatePermissionPage createRolePermissionPage = WebPageFactory.Create<CreatePermissionPage>(page.WebDriver);
            return createRolePermissionPage;
        }
    }
}
