using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.Docking;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Questions.Keys;
using CMDB.UI.Specflow.Questions.Main;
using CMDB.UI.Specflow.Tasks;
using Reqnroll;

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
        public async Task<Kensington> CreateKensington()
        {
            return await Perform(new CreateTheKensington());
        }
        public async Task AssignKey(Docking docking, Kensington kensington)
        {
            var context = GetAbility<DataContext>();
            await context.AssignDevice2Key(admin, docking, kensington);
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
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignIdentityPage");
            page.SelectIdentity(identity);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedIdentity");
        }
        public void FillInAssignForm(Identity identity)
        {
            if (ScenarioContext.ScenarioInfo.Title.Contains("assign an existing Identiy")) { 
                var assignForm = OpenAssignFom();
                assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
                assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
                Perform<ClickTheGeneratePDFOnAssignForm>();
                assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
            }
            else
            {
                var keyassignForm = Perform(new OpenTheKensingtonAssignFormPage());
                keyassignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignForm");
                keyassignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
                keyassignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
                keyassignForm.CreatePDF();
                keyassignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
            }
        }
        public void DoReleaseIdentityFromDocking(Identity identity, Docking docking)
        {
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine($"{docking.Category.Category} with {docking.AssetTag}",
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheDockingDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheDockingReleaseIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
        public void DoAssignTheKey2Docking(Docking docking, Kensington key)
        {
            Search(docking.AssetTag);
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{docking.Category.Category} with {docking.AssetTag}";
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, keyinfo, admin.Account.UserID, Table);
            var page = Perform(new OpenTheDockingAssignKensingtonPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignKensingtonPage");
            page.SelectKensington(key);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedKensington");
        }
        public void DoReleaseKey4Docking(Docking docking, Kensington key, Identity identity)
        {
            Search(docking.AssetTag);
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{docking.Category.Category} with {docking.AssetTag}";
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, keyinfo, admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheDockingDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheDockingReleaseKensingtonPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseKensingtonPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
    }
}
