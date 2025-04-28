using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Desktop
{
    public class OpenTheDesktopCreatePage : Question<CreateDesktopPage>
    {
        public override CreateDesktopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            CreateDesktopPage createDesktopPage = WebPageFactory.Create<CreateDesktopPage>(page.WebDriver);
            return createDesktopPage;
        }
    }
}
