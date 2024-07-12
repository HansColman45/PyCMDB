using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Docking;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.Dockings
{
    public class DockingIdentityActor : DockingUpdator
    {
        public DockingIdentityActor(ScenarioContext scenarioContext, string name = "DockingIdentityActor") : base(scenarioContext, name)
        {
        }
        public async Task<Identity> CreateNewIdentity()
        {
            return await Perform(new CreateTheIdentity());
        }
        public async Task AssignIdenity2Docking(Identity identity, Docking docking)
        {
            try
            {
                var context = GetAbility<DataContext>();
                await context.AssignIdentity2Device(admin, docking, identity);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public void DoAssignIdentityt2Docking(Identity identity, Docking docking) 
        {
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine($"{docking.Category.Category} with {docking.AssetTag}",
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheDockingAssignIdentityPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignIdentityPage");
            page.SelectIdentity(identity);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedIdentity");
        }
        public void FillInAssignForm(Identity identity)
        {
            var assignForm = OpenAssignFom();
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            Perform<ClickTheGeneratePDFOnAssignForm>();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
        }
        public void DoReleaseIdentityFromDocking(Identity identity, Docking docking)
        {
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine($"{docking.Category.Category} with {docking.AssetTag}",
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheDockingDetailPage());
            detailPage.WebDriver = Driver;
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheDockingReleaseIdentityPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
    }
}
