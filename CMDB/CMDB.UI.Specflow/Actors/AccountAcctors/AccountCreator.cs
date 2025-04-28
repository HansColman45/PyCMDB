using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Questions.Account;
using CMDB.Infrastructure;

namespace CMDB.UI.Specflow.Actors.AccountAcctors
{
    public class AccountCreator : AccountActor
    {
        public AccountCreator(ScenarioContext scenarioContext, string name = "AccountCreator") : base(scenarioContext, name)
        {
        }
        public void CreateNewAccount(Helpers.Account account)
        {
            rndNr = rnd.Next();
            var createPage = Perform(new OpenTheAccountCreatePage());
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            createPage.UserId = account.UserId + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UserId");
            createPage.Type = account.Type;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            createPage.Application = account.Application;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Application");
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
            log.Info($"We created an account with UserID: {account.UserId + rndNr} and Type: {account.Type}");
        }
        public void SearchAccount(Helpers.Account account)
        {
            var page = GetAbility<AccountOverviewPage>();
            page.Search(account.UserId + rndNr.ToString());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Account with UserID: {account.UserId+rndNr} and type {account.Type} for application {account.Application}",
                admin.Account.UserID,
                Table);
        }
    }
}
