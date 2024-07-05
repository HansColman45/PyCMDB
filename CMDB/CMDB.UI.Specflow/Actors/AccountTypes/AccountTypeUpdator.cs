using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Pages.Types;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Types;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.AccountTypes
{
    public class AccountTypeUpdator : AccountTypeActor
    {
        public AccountTypeUpdator(ScenarioContext scenarioContext, string name = "AcountTypeUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<AccountType> CreateNewAccountType(bool active = true)
        {
            if(active)
                return await Perform(new CreateTheAccountType());
            else
                return await Perform(new CreateTheInactiveAccountType());
        }
        public AccountType UpdateAccountType(AccountType accountType, string field, string value)
        {
            rndNr = rnd.Next();
            var page = Perform(new OpenTheTypeEditPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            switch (field)
            {
                case "Type":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, accountType.Type, value + rndNr.ToString(), admin.Account.UserID, Table);
                    page.Type = value + rndNr.ToString();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_type");
                    accountType.Type = value + rndNr.ToString();
                    break;
                case "Description":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, accountType.Description, value + rndNr.ToString(), admin.Account.UserID, Table);
                    page.Description = value + rndNr.ToString();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_description");
                    accountType.Description = value + rndNr.ToString();
                    break;
                default:
                    log.Fatal($"The update on {field} is not implemented");
                    break;
            }
            page.Edit();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_updated");
            return accountType;
        }
        public void DeactivateAccount(AccountType accountType, string reason)
        {
            var page = Perform(new OpenTheTypeDeactivatePage());
			page.WebDriver = Driver;
			page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
			page.Reason = reason;
			page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_reason");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Accounttype with type: {accountType.Type} and description: {accountType.Description}", admin.Account.UserID, reason,Table);
			page.Delete();
			page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deactivated");
        }
        public void ActivateAccount(AccountType accountType)
        {
            var page = GetAbility<TypeOverviewPage>();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_activated");
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Accounttype with type: {accountType.Type} and description: {accountType.Description}", admin.Account.UserID,Table);
        }
    }
}
