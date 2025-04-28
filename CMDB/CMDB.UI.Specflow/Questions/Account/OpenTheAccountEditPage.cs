using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Account
{
    public class OpenTheAccountEditPage : Question<EditAccountPage>
    {
        public override EditAccountPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountOverviewPage>();
            page.ClickElementByXpath(AccountOverviewPage.EditXpath);
            page.WaitUntilElmentVisableByXpath("//button[.='Edit']");
            EditAccountPage editAccount = WebPageFactory.Create<EditAccountPage>(page.WebDriver);
            return editAccount;
        }
    }
}
