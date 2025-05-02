using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;

namespace CMDB.UI.Specflow.Questions.Desktop
{
    public class OpenTheDesktopAssignKensingtonPage : Question<DesktopAssignKensingtonPage>
    {
        public override DesktopAssignKensingtonPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(MainPage.AssignKensingtonXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            DesktopAssignKensingtonPage desktopAssignKensingtonPage = WebPageFactory.Create<DesktopAssignKensingtonPage>(page.WebDriver);
            return desktopAssignKensingtonPage;
        }
    }
}
