using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Admin;

namespace CMDB.UI.Specflow.Questions.Admin
{
    public class OpenTheAdminDeactivatePage : Question<DeactivateAdminPage>
    {
        public override DeactivateAdminPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AdminOverviewPage>();
            page.ClickElementByXpath(MainPage.DeactivateXpath);
            DeactivateAdminPage deactivatePage = WebPageFactory.Create<DeactivateAdminPage>(page.WebDriver);
            return deactivatePage;
        }
    }
}
