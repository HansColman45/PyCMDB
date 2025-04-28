using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Account
{
    public class OpenTheAccountDeactivatePage : Question<DeactivateAccountPage>
    {
        public override DeactivateAccountPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(MainPage.DeactivateXpath);
            DeactivateAccountPage deactivateAccount = WebPageFactory.Create<DeactivateAccountPage>(page.WebDriver);
            return deactivateAccount;
        }
    }
}
