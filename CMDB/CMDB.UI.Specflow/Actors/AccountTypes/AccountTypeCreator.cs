using CMDB.UI.Specflow.Abilities.Pages.Types;
using CMDB.UI.Specflow.Helpers;
using CMDB.UI.Specflow.Questions.Types;

namespace CMDB.UI.Specflow.Actors.AccountTypes
{
    public class AccountTypeCreator : AccountTypeActor
    {
        public AccountTypeCreator(ScenarioContext scenarioContext, string name = "AccountAcctor") : base(scenarioContext, name)
        {
        }
        public void CreateAccountType(AccountType accountType)
        {
            rndNr = rnd.Next();
            var page = Perform(new OpenTheTypeCreatePage());
            page.WebDriver= Driver;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_create");
            page.Type = accountType.Type + rndNr.ToString();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_type");
            page.Description = accountType.Description + rndNr.ToString();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_description");
            page.Create();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_created");
        }
        public void SearchAccountType(AccountType accountType)
        {
            var page = GetAbility<TypeOverviewPage>();
            page.Search(accountType.Type + rndNr.ToString());
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = $"The Accounttype with type: {accountType.Type + rndNr.ToString()} and description: {accountType.Description + rndNr.ToString()} " +
                $"is created by {admin.Account.UserID} in table accounttype";
        }
    }
}
