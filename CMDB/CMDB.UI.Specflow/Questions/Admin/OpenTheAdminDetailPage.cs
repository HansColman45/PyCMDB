using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Admin;

namespace CMDB.UI.Specflow.Questions.Admin
{
    public class OpenTheAdminDetailPage: Question<AdminDetailPage>
    {
        public override AdminDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AdminOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            AdminDetailPage adminDetailPage = WebPageFactory.Create<AdminDetailPage>(page.WebDriver);
            return adminDetailPage;
        }
    }
}
