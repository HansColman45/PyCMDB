using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Permissions;

namespace CMDB.UI.Specflow.Questions.Permissions
{
    public class OpenTheDeletePermissionPage : Question<DeletePermissionPage>
    {
        public override DeletePermissionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<PermissionOverviewPage>();
            page.ClickElementByXpath("//a[@title='Delete']");
            DeletePermissionPage deletePermissionPage = WebPageFactory.Create<DeletePermissionPage>(page.WebDriver);
            return deletePermissionPage;
        }
    }
}
