
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Questions.Mobile;
using CMDB.UI.Specflow.Tasks;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Mobiles
{
    public class MobileIdentityActor : MobileUpdator
    {
        public MobileIdentityActor(ScenarioContext scenarioContext, string name = "MobileIdentityActor") : base(scenarioContext, name)
        {
        }
        public async Task<Identity> CreateNewIdentity()
        {
            return await Perform(new CreateTheIdentity());
        }
        public async Task AssignIdentity(Mobile mobile, Identity identity)
        {
            var context = GetAbility<DataContext>();
            await context.AssignIdentity2Mobile(admin, mobile, identity);
        }
        public void AssignMobile2Identity(Mobile mobile, Identity identity) 
        {
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine($"mobile with type {mobile.MobileType}",
                $"Identity with name: {identity.Name}", admin.Account.UserID,Table);
            var page = Perform(new OpenTheMobileAssignIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignIdentityPage");
            page.SelectIdentity(identity);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectIdenity");
        }
        public void FillInAssignForm(Identity identity)
        {
            var assignForm = OpenAssignFom();
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            Perform<ClickTheGeneratePDFOnAssignForm>();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
        }

        public void ReleaseIdentity(Mobile mobile, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine($"mobile with type {mobile.MobileType}",
               $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheMobileDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheMobileReleaseIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
    }
}
