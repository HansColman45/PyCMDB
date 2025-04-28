using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Types;

namespace CMDB.UI.Specflow.Questions.Types
{
    public class OpenTheTypeDeactivatePage : Question<DeactivateTypePage>
    {
        public override DeactivateTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(MainPage.DeactivateXpath);
            DeactivateTypePage deactivateTypePage = WebPageFactory.Create<DeactivateTypePage>(page.WebDriver);
            return deactivateTypePage;
        }
    }
}
