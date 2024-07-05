using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Questions.Account;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.AccountAcctors
{
    public class AccountCreator : AccountActor
    {
        public AccountCreator(ScenarioContext scenarioContext, string name = "AccountCreator") : base(scenarioContext, name)
        {
        }
        public CreateAccountPage OpenAccountCreatePage()
        {
            var createPage = Perform(new OpenTheAccountCreatePage());
            createPage.WebDriver = Driver;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            return createPage;
        }
        public void CreateNewAccount(Helpers.Account account)
        {
            rndNr = rnd.Next();
            var page = GetAbility<CreateAccountPage>();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            page.UserId = account.UserId + rndNr.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UserId");
            page.Type = account.Type;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            page.Application = account.Application;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Application");
            page.Create();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
            log.Info($"We created an account with UserID: {account.UserId + rndNr} and Type: {account.Type}");
        }
        public void SearchAccount(Helpers.Account account)
        {
            var page = GetAbility<AccountOverviewPage>();
            page.Search(account.UserId + rndNr.ToString());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Account with UserID: {account.UserId+rndNr} and with type {account.Type} for application {account.Application}",
                admin.Account.UserID,
                Table);
        }
    }
}
