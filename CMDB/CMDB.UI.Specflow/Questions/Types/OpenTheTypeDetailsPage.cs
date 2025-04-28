using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Types;

namespace CMDB.UI.Specflow.Questions.Types
{
    public class OpenTheTypeDetailsPage : Question<TypeDetailPage>
    {
        public override TypeDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<TypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            TypeDetailPage typeDetailPage = WebPageFactory.Create<TypeDetailPage>(page.WebDriver);
            return typeDetailPage;
        }
    }
}
