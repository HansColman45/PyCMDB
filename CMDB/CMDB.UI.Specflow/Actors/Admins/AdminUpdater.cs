
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Admin;
using CMDB.UI.Specflow.Questions.Admin;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Admins
{
    public class AdminUpdater : AdminCreator
    {
        public AdminUpdater(ScenarioContext scenarioContext, string name = "AdminUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Admin> CreateAdminForAccount(Account account, bool active = true)
        {
            var datacontext = GetAbility<DataContext>();
            Admin admin = await datacontext.CreateNewAdmin(account, active: active);
            return admin;
        }
        public void DoUpdateAdmin(Admin newAdmin, string level)
        {
            ExpectedLog = GenericLogLineCreator.UpdateLogLine("level", "9", level, admin.Account.UserID, "admin");
            var updatepage = Perform(new OpenTheAdminUpdatePage());
            updatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_update");
            updatepage.Level = level;
            updatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LevelSelected");
            updatepage.Edit();
        }

        public void Deactivate(Admin newAdmin, string reason)
        {
            string value = "Admin with UserID: " + newAdmin.Account.UserID;
            ExpectedLog = GenericLogLineCreator.DeleteLogLine(value, admin.Account.UserID, reason, "admin");
            var deactivatePage = Perform(new OpenTheAdminDeactivatePage());
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deactivate");
            deactivatePage.Reason = reason;
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            deactivatePage.Delete();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deleted");
        }

        public void Activate(Admin newAdmin)
        {
            string value = "Admin with UserID: " + newAdmin.Account.UserID;
            ExpectedLog = GenericLogLineCreator.ActivateLogLine(value, admin.Account.UserID, "admin");
            var page = GetAbility<AdminOverviewPage>();
            page.ClickElementByXpath(MainPage.ActivateXpath);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_activate");
        }
    }
}
