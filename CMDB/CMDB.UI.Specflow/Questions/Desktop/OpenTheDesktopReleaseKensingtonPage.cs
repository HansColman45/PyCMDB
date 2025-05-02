using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;

namespace CMDB.UI.Specflow.Questions.Desktop
{
    public class OpenTheDesktopReleaseKensingtonPage: Question<DesktopReleaseKensingtonPage>
    {
        public override DesktopReleaseKensingtonPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(MainPage.ReleaseKensingtonXpath);
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            DesktopReleaseKensingtonPage desktopReleaseKensingtonPage = WebPageFactory.Create<DesktopReleaseKensingtonPage>(page.WebDriver);
            return desktopReleaseKensingtonPage;
        }
    }
}
