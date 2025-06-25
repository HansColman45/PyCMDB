using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.Account;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Tasks;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.AccountAcctors
{
    public class AccountIdentityActor : AccountUpdator
    {
        public AccountIdentityActor(ScenarioContext scenarioContext, string name = "AccountIdentityActor") : base(scenarioContext, name)
        {
        }
        public async Task<Identity> CreateNewIdentity()
        {
            return await Perform(new CreateTheIdentity());
        }
        public async Task AssignAccount2Identity(Account account, Identity identity)
        {
            var context = GetAbility<DataContext>();
            await context.AssignIden2Account(identity, account, admin);
        }
        public void AssignTheIdentity2Account(Account account, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.AssingAccount2IdenityLogLine($"Account with UserID: {account.UserID}",
                $"Identity with name: {identity.Name}",admin.Account.UserID,Table);
            var page = Perform(new OpenTheAccountAssignIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignIdentityPage");
            page.SelectIdentity(identity);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedIdentity");
            DateTime validFrom, validUntil;
            validFrom = DateTime.UtcNow.AddYears(-1);
            validUntil = DateTime.UtcNow.AddYears(1);
            page.ValidFrom = validFrom;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ValidFrom");
            page.ValidUntil = validUntil;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ValidUntil");
        }
        public void FillInAssignForm()
        {
            var page = Perform(new OpenTheAccountAssignFormPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignFormPage");
            Perform<ClickTheGeneratePDFOnAssignForm>();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
        }
        public void ReleaseIdentity(Account account, Identity identity)
        {   
            var detailPage = Perform(new OpenTheAccountDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            ExpectedLog = GenericLogLineCreator.ReleaseAccountFromIdentityLogLine($"Account with UserID: {account.UserID}",
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheAccountReleaseIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            page.CreatePDF();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Released");
        }
    }
}
