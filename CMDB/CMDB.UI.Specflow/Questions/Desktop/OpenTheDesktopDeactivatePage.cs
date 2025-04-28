using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Desktop
{
    public class OpenTheDesktopDeactivatePage : Question<DeactivateDesktopPage>
    {
        public override DeactivateDesktopPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<DesktopOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            DeactivateDesktopPage deactivateDesktopPage = WebPageFactory.Create<DeactivateDesktopPage>(page.WebDriver);
            return deactivateDesktopPage;
        }
    }
}
