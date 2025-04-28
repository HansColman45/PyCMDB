using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Types;

namespace CMDB.UI.Specflow.Questions.Types
{
    public class OpenTheTypeEditPage : Question<UpdateTypePage>
    {
        public override UpdateTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            UpdateTypePage updateTypePage = WebPageFactory.Create<UpdateTypePage>(page.WebDriver);
            return updateTypePage;
        }
    }
}
