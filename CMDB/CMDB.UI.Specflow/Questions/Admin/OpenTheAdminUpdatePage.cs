using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Admin;

namespace CMDB.UI.Specflow.Questions.Admin
{
    public class OpenTheAdminUpdatePage : Question<UpdateAdminPage>
    {
        public override UpdateAdminPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AdminOverviewPage>();
            page.ClickElementByXpath(MainPage.EditXpath);
            UpdateAdminPage updatePage = WebPageFactory.Create<UpdateAdminPage>(page.WebDriver);
            return updatePage;
        }
    }
}
