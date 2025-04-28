using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Desktop
{
    public class OpenTheDesktopDetailPage : Question<DesktopDetailPage>
    {
        public override DesktopDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            DesktopDetailPage desktopDetailPage = WebPageFactory.Create<DesktopDetailPage>(page.WebDriver);
            return desktopDetailPage;
        }
    }
}
