using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Tasks;
using CMDB.UI.Specflow.Tasks.Account;

namespace CMDB.UI.Specflow.Actors
{
    public class AccountCreator : CMDBActor
    {
        public AccountCreator(ScenarioContext scenarioContext, string name = "AccountCreator") : base(scenarioContext, name)
        {
        }
        public void CreateNewAccount(Helpers.Account account)
        {
            rndNr = rnd.Next();
            var page = GetAbility<CreateAccountPage>();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_create");
            page.UserId = account.UserId + rndNr.ToString();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_UserId");
            page.Type = account.Type;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Type");
            page.Application = account.Application;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Application");
        }
        public void SearchAccount(Helpers.Account account)
        {
            var page = GetAbility<AccountOverviewPage>();
            page.Search(account.UserId + rndNr.ToString());
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = $"The Account width name: {account.UserId + rndNr.ToString()} is created by {admin.Account.UserID} in table account";
        }
    }
}
