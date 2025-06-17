
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Questions.Account;
using CMDB.UI.Specflow.Questions.Admin;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Admins
{
    public class AdminCreator : AdminActor
    {
        public AdminCreator(ScenarioContext scenarioContext, string name = "AdminActor") : base(scenarioContext, name)
        {
        }

        public async Task<Account> CreateAccount()
        {
            return await Perform(new CreateTheAccount());
        }

        public void DoCreateNewAdmin(Account account, string level)
        {
            string value = "Admin with UserID: " + account.UserID + " and level: " + level;
            ExpectedLog = GenericLogLineCreator.CreateLogLine(value, admin.Account.UserID,"admin");
            var createpage = Perform(new OpenTheAdminCreatePage());
            createpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            createpage.SelectAccount(account);
            createpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AdminSelected");
            createpage.Level = level;
            createpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LevelSelected");
            createpage.Create();
        }
    }
}
