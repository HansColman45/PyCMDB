using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Docking;
using CMDB.UI.Specflow.Questions.Main;
using CMDB.UI.Specflow.Questions.Monitor;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.Monitors
{
    public class MonitorIdentityActor : MonitorUpdator
    {
        public MonitorIdentityActor(ScenarioContext scenarioContext, string name = "MonitorIdentityActor") : base(scenarioContext, name)
        {
        }
        public async Task<Identity> CreateNewIdentity()
        {
            return await Perform(new CreateTheIdentity());
        }
        public async Task AssignIdentity(Screen monitor, Identity identity)
        {
            try
            {
                var context = GetAbility<DataContext>();
                await context.AssignIdentity2Device(admin, monitor, identity);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public async Task<Kensington> CreateKensington()
        {
            return await Perform(new CreateTheKensington());
        }
        public async Task AssignKey(Screen monitor, Kensington kensington)
        {
            var context = GetAbility<DataContext>();
            await context.AssignDevice2Key(admin, monitor, kensington);
        }
        public void AssignTheIdentity2Monitor(Screen screen, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine($"Monitor with {screen.AssetTag}",
               $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheMonitorAssignIdentityPage());
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
        public void ReleaseIdentity(Screen screen, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine($"Monitor with {screen.AssetTag}",
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheMonitorDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheMonitorReleaseIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
        public void DoAssignTheKey2Monitor(Screen screen, Kensington key)
        {
            Search(screen.AssetTag);
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{screen.Category.Category} with {screen.AssetTag}";
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, keyinfo, admin.Account.UserID, Table);
            var page = Perform(new OpenTheMonitorAssignKensingtonPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignKensingtonPage");
            page.SelectKensington(key);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedKensington");
        }
        public void DoReleaseKey4Monitor(Screen screen, Kensington key, Identity identity)
        {
            Search(screen.AssetTag);
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{screen.Category.Category} with {screen.AssetTag}";
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, keyinfo, admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheMonitorDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheMonitorReleaseKensingtonPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseKensingtonPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
    }
}
