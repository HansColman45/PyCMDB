using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Questions.Account;

namespace CMDB.UI.Specflow.Actors
{
    public class AccountUpdator : AccountActor
    {
        public AccountUpdator(ScenarioContext scenarioContext, string name = "AccountUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Account> CreateAccount(bool active = false)
        {
            var db = GetAbility<DataContext>();
            return await db.CreateAccount(admin, active);
        }
        public EditAccountPage OpenEditAccountPage()
        {
            var page = Perform(new OpenTheAccountEditPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_EditPage");
            return page;
        }
        public Account UpdateAccount(string field, string value, Account account)
        {
            rndNr = rnd.Next();
            var page = OpenEditAccountPage();
            switch (field)
            {
                case "UserId":
                    ExpectedLog = $"The {field} has been changed from {account.UserID} to {value + rndNr.ToString()} by {admin.Account.UserID} in table account";
                    page.UserId = value + rndNr.ToString();
                    account.UserID = value + rndNr.ToString();
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_UserId");
                    break;
                case "Type":
                    ExpectedLog = $"The {field} has been changed from {account.Type.Type} to {value} by {admin.Account.UserID} in table account";
                    page.Type = value;
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Type");
                    break;
                case "Application":
                    ExpectedLog = $"The {field} has been changed from {account.Application.Name} to {value} by {admin.Account.UserID} in table account";
                    page.Application = value;
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Application");
                    break;
                default:
                    log.Fatal($"Update on field {field} is not supported");
                    throw new Exception($"Update on field {field} is not supported");
            }
            page.Edit();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Changed");
            return account;
        }
        public void DeactivateAccount(Account account, string reason)
        {
            var page = Perform(new OpenTheAccountDeactivatePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_DeactivatePage");
            page.Reason = reason;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Reason");
            ExpectedLog = $"The Account width UserID: {account.UserID} and type {account.Type.Description} is deleted due to {reason} by {admin.Account.UserID} in table account";
            page.Delete();
        }
        public void AcctivateAccount(Account account) 
        {
            var page = GetAbility<AccountOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Activated");
            ExpectedLog = $"The Account width UserID: {account.UserID} and type {account.Type.Description} is activated by {admin.Account.UserID} in table account";
        }
    }
}
