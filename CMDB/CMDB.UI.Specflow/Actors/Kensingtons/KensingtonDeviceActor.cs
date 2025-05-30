using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Keys;
using CMDB.UI.Specflow.Tasks;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Kensingtons
{
    public class KensingtonDeviceActor : KensingtonUpdator
    {
        public KensingtonDeviceActor(ScenarioContext scenarioContext, string name = "KensingtonDeviceActor") : base(scenarioContext, name)
        {
        }
        public async Task<Device> CreateNewDevice(string category)
        {
            return category switch
            {
                "Desktop" => await Perform(new CreateTheDesktop()),
                "Laptop" => await Perform(new CreateTheLaptop()),
                "Docking" => await Perform(new CreateTheDockingStation()),
                "Monitor" => await Perform(new CreateTheMonitor()),
                "Screen" => await Perform(new CreateTheMonitor()),
                _ => throw new Exception($"Category {category} not found")
            };
        }
        public async Task<Identity> CreateNewIdentity()
        {
            return await Perform(new CreateTheIdentity());
        }
        public async Task AssignDevice2Identity(Device device, Identity identity)
        {
            var context = GetAbility<DataContext>();
            await context.AssignIdentity2Device(admin, device, identity);
        }
        public async Task AssignDevice2Key(Device device, Kensington kensington)
        {
            var context = GetAbility<DataContext>();
            await context.AssignDevice2Key(admin, device, kensington);
        }
        public void DoAssignKey2Device(Kensington kensington, Device device)
        {
            string keyinfo = $"Kensington with serial number: {kensington.SerialNumber}";
            string deviceinfo = $"{device.Category.Category} with {device.AssetTag}";
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine(keyinfo, deviceinfo, admin.Account.UserID, Table);
            var page = Perform(new OpenTheKensingtonAssignDevicePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignDevicePage");
            page.SelectDevice(device);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedDevice");
        }
        public void FillInAssignForm(Identity identity)
        {
            var assignForm = OpenAssignFom();
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            AttemptsTo<ClickTheGeneratePDFOnAssignForm>();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
        }
        public void DoReleaseDevice(Kensington kensington, Device device, Identity identity)
        {
            string keyinfo = $"Kensington with serial number: {kensington.SerialNumber}";
            string deviceinfo = $"{device.Category.Category} with {device.AssetTag}";
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(keyinfo, deviceinfo, admin.Account.UserID, Table);
            var detailpage = Perform(new OpenTheKensingtonDetailPage());
            detailpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheKensingtonReleaseDevicePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseDevicePage");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.CreatePDF();
        }
    }
}
