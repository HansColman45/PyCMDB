using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Permissions;

namespace CMDB.UI.Specflow.Questions.Permissions
{
    public class OpenTheEditPermissionPage : Question<EditPermissionPage>
    {
        public override EditPermissionPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<PermissionOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            EditPermissionPage editPage = WebPageFactory.Create<EditPermissionPage>(page.WebDriver);
            return editPage;
        }
    }
}
