using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Types;

namespace CMDB.UI.Specflow.Questions.Types
{
    public class OpenTheTypeCreatePage : Question<CreateTypePage>
    {
        public override CreateTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreateTypePage createTypePage = WebPageFactory.Create<CreateTypePage>(page.WebDriver);
            return createTypePage;
        }
    }
}