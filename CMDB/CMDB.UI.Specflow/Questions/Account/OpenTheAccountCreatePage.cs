using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Account
{
    public class OpenTheAccountCreatePage : Question<CreateAccountPage>
    {
        public override CreateAccountPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(MainPage.NewXpath);
            CreateAccountPage createAccount = WebPageFactory.Create<CreateAccountPage>(page.WebDriver);
            return createAccount;
        }
    }
}
