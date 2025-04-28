using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;

namespace CMDB.UI.Specflow.Questions.Mobile
{
    public class OpenTheMobileDetailPage : Question<MobileDetailPage>
    {
        public override MobileDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MobileOverviewPage>();
            page.ClickElementByXpath(MainPage.InfoXpath);
            MobileDetailPage mobileDetailPage = WebPageFactory.Create<MobileDetailPage>(page.WebDriver);
            return mobileDetailPage;
        }
    }
}
