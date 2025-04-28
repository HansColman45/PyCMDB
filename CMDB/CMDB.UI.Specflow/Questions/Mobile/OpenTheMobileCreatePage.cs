using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    public class OpenTheMobileCreatePage : Question<CreateMobilePage>
    {
        public override CreateMobilePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreateMobilePage createMobilePage = WebPageFactory.Create<CreateMobilePage>(page.WebDriver);
            return createMobilePage;
        }
    }
}
