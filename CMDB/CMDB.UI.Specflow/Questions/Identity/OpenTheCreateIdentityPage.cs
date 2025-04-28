using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheCreateIdentityPage : Question<CreateIdentityPage>
    {
        public override CreateIdentityPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreateIdentityPage createIdentityPage = WebPageFactory.Create<CreateIdentityPage>(page.WebDriver);
            return createIdentityPage;
        }
    }
}
