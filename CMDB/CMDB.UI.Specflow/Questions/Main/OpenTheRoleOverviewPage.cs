using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Role;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheRoleOverviewPage : Question<RoleOverviewPage>
    {
        public override RoleOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Role']");
            page.ClickElementByXpath("//a[@id='Role']");
            page.ClickElementByXpath("//a[@id='Role8']");
            page.ClickElementByXpath("//a[@href='/Role']");
            page.WaitOnAddNew();
            RoleOverviewPage roleOverviewPage = WebPageFactory.Create<RoleOverviewPage>(page.WebDriver);
            return roleOverviewPage;
        }
    }
}
