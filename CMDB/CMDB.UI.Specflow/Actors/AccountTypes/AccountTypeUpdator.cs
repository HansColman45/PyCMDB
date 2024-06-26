using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages.Types;
using CMDB.UI.Specflow.Questions.Types;

namespace CMDB.UI.Specflow.Actors.AccountTypes
{
    public class AccountTypeUpdator : AccountTypeActor
    {
        public AccountTypeUpdator(ScenarioContext scenarioContext, string name = "AcountTypeUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<AccountType> CreateNewAccountType(bool active = true)
        {
            return await GetAbility<DataContext>().CreateAccountType(admin, active);
        }
        public AccountType UpdateAccountType(AccountType accountType, string field, string value)
        {
            rndNr = rnd.Next();
            var page = Perform(new OpenTheTypeEditPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
            switch (field)
            {
                case "Type":
                    page.Type = value + rndNr.ToString();
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_type");
                    ExpectedLog = $"The {field} has been changed from {accountType.Type} to {value + rndNr.ToString()} by {admin.Account.UserID} in table accounttype";
                    accountType.Type = value + rndNr.ToString();
                    break;
                case "Description":
                    page.Description = value + rndNr.ToString();
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_description");
                    ExpectedLog = $"The {field} has been changed from {accountType.Description} to {value + rndNr.ToString()} by {admin.Account.UserID} in table accounttype";
                    accountType.Description = value + rndNr.ToString();
                    break;
                default:
                    break;
            }
            page.Edit();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_updated");
            return accountType;
        }
        public void DeactivateAccount(AccountType accountType, string reason)
        {
            var page = Perform(new OpenTheTypeDeactivatePage());
			page.WebDriver = Driver;
			page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
			page.Reason = reason;
			page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_reason");
			ExpectedLog = $"The Accounttype with type: {accountType.Type} and description: {accountType.Description} is deactivated due to {reason} " +
                $"by {admin.Account.UserID} in table accounttype";
			page.Delete();
			page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_deactivated");
        }
        public void ActivateAccount(AccountType accountType)
        {
            var page = GetAbility<TypeOverviewPage>();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
            page.Activate();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_activated");
            ExpectedLog = $"The Accounttype with type: {accountType.Type} and description: {accountType.Description} " +
                $"is activated by {admin.Account.UserID} in table accounttype";
        }
    }
}
