using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Admin;

namespace CMDB.UI.Specflow.Questions.Admin
{
    public class OpenTheAdminCreatePage : Question<CreateAdminPage>
    {
        public override CreateAdminPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AdminOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreateAdminPage createAdminPage = WebPageFactory.Create<CreateAdminPage>(page.WebDriver);
            return createAdminPage;
        }
    }
}
