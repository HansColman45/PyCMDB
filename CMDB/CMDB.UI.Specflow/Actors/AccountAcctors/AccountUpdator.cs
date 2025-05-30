using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Questions.Account;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.AccountAcctors
{
    public class AccountUpdator : AccountActor
    {
        public AccountUpdator(ScenarioContext scenarioContext, string name = "AccountUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Account> CreateAccount(bool active = true)
        {
            if (active)
                return await Perform(new CreateTheAccount());
            else
                return await Perform(new CreateTheIncativeAccount());
        }
        public EditAccountPage OpenEditAccountPage()
        {
            var page = Perform(new OpenTheAccountEditPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
            return page;
        }
        public Account UpdateAccount(string field, string value, Account account)
        {
            rndNr = rnd.Next();
            var page = OpenEditAccountPage();
            switch (field)
            {
                case "UserId":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, account.UserID, value + rndNr.ToString(), admin.Account.UserID, Table);
                    page.UserId = value + rndNr.ToString();
                    account.UserID = value + rndNr.ToString();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UserId");
                    break;
                case "Type":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, account.Type.Type, value, admin.Account.UserID, Table);
                    page.Type = value;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
                    break;
                case "Application":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, account.Application.Name, value, admin.Account.UserID, Table);
                    page.Application = value;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Application");
                    break;
                default:
                    log.Fatal($"Update on field {field} is not supported");
                    throw new Exception($"Update on field {field} is not supported");
            }
            page.Edit();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Changed");
            return account;
        }
        public void DeactivateAccount(Account account, string reason)
        {
            var page = Perform(new OpenTheAccountDeactivatePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Account with UserID: {account.UserID} and type {account.Type.Description}", admin.Account.UserID,reason,Table);
            page.Delete();
        }
        public void AcctivateAccount(Account account)
        {
            var page = GetAbility<AccountOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Account with UserID: {account.UserID} and type {account.Type.Description}", admin.Account.UserID,Table);
        }
    }
}
