
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.IdentityActors
{
    public class IdentityAccountActor : IdentityUpdator
    {
        public IdentityAccountActor(ScenarioContext scenarioContext, string name = "IdentityAccountActor") : base(scenarioContext, name)
        {
        }
        public async Task<Account> CreateAccount()
        {
            return await Perform(new CreateTheAccount());
        }
        public async Task AssignAccount(Identity identity, Account account)
        {
            try
            {
                var context = GetAbility<DataContext>();
                await context.AssignIden2Account(identity, account,admin);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public void AssignIdentity2Account(Identity identity, Account account)
        {
            string accountinfo = $"Identity with name: {identity.FirstName}, {identity.LastName}";
            string idenInfo = $"Account with UserID: {account.UserID}";
            ExpectedLog = GenericLogLineCreator.AssingAccount2IdenityLogLine(accountinfo, idenInfo, admin.Account.UserID,Table);
            var page = Perform(new OpenTheAssignAccountPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignAccountPage");
            page.SelectAccount(account);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AccountSelected");
            page.ValidFrom = DateTime.Now.AddDays(-1);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_StartDateEntered");
            page.ValidUntil = DateTime.Now.AddYears(+2);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EndDateEntered");
        }
        public void FillInAssignForm(Identity identity) 
        {
            var assignForm = OpenAssignFom();
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            Perform<ClickTheGeneratePDFOnAssignForm>();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
        }

        public void ReleaseAccount(Identity identity, Account account)
        {
            string accountinfo = $"Identity with name: {identity.FirstName}, {identity.LastName}";
            string idenInfo = $"Account with UserID: {account.UserID}";
            ExpectedLog = GenericLogLineCreator.ReleaseAccountFromIdentityLogLine(accountinfo,idenInfo,admin.Account.UserID,Table);
            var detailpage = Perform(new OpenTheIdentityDetailPage());
            detailpage.WebDriver = Driver;
            detailpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var releasepage = Perform(new OpenTheReleaseAccountPage());
            releasepage.WebDriver = Driver;
            releasepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            releasepage.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            releasepage.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            releasepage.CreatePDF();
        }
    }
}
