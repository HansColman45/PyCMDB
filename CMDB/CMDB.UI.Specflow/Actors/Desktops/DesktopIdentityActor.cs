using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.Desktop;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Questions.Keys;
using CMDB.UI.Specflow.Questions.Main;
using CMDB.UI.Specflow.Tasks;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Desktops
{
    public class DesktopIdentityActor : DesktopUpdator
    {
        public DesktopIdentityActor(ScenarioContext scenarioContext, string name = "DesktopUpdator") : base(scenarioContext, name)
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
        public async Task AssignKey(Desktop desktop, Kensington kensington)
        {
            var context = GetAbility<DataContext>();
            await context.AssignDevice2Key(admin, desktop, kensington);
        }
        public async Task AssignDesktop2Identity(Desktop desktop, Identity identity)
        {
            try
            {
                var context = GetAbility<DataContext>();
                await context.AssignIdentity2Device(admin, desktop, identity);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public void AssignTheIdentity2Desktop(Desktop desktop, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine($"Desktop with {desktop.AssetTag}",
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheDesktopAssignIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignIdentityPage");
            page.SelectIdentity(identity);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedIdentity");
        }
        public void FillInAssignForm(Identity identity)
        {
            if (ScenarioContext.ScenarioInfo.Title.Contains("assign an existing Identiy"))
            {
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
        public void ReleaseIdentity(Desktop desktop, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine($"Desktop with {desktop.AssetTag}",
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheDesktopDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheDesktopReleaseIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
        
        public void DoAssignTheKey2Desktop(Desktop desktop, Kensington key)
        {
            Search(desktop.AssetTag);
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{desktop.Category.Category} with {desktop.AssetTag}";
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, keyinfo, admin.Account.UserID, Table);
            var page = Perform(new OpenTheDesktopAssignKensingtonPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignKensingtonPage");
            page.SelectKensington(key);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedKensington");
        }
        public void DoReleaseKey4Desktop(Desktop desktop, Kensington key, Identity identity)
        {
            Search(desktop.AssetTag);
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{desktop.Category.Category} with {desktop.AssetTag}";
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, keyinfo, admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheDesktopDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheDesktopReleaseKensingtonPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseKensingtonPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
    }
}
