using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.RolePermission;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheRolePermissionOverviewPage : Question<RolePermissionOverviewPage>
    {
        public override RolePermissionOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Role permission51']");
            page.ClickElementByXpath("//a[@href='/RolePermission']");
            page.WaitOnAddNew();
            RolePermissionOverviewPage permissionOverviewPage = WebPageFactory.Create<RolePermissionOverviewPage>(page.WebDriver);
            return permissionOverviewPage;
        }
    }
}
