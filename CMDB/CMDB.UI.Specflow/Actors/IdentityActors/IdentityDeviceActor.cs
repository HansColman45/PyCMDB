


using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Tasks;
using System.Runtime.Serialization.DataContracts;

namespace CMDB.UI.Specflow.Actors.IdentityActors
{
    public class IdentityDeviceActor : IdentityUpdator
    {
        public IdentityDeviceActor(ScenarioContext scenarioContext, string name = "IdentityDeviceActor") : base(scenarioContext, name)
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
                "Token" => await Perform(new CreateTheToken()),
                _ => throw new Exception($"Category {category} not found")
            };
        }
        public async Task AssignDevice2Identity(Device device, Identity identity)
        {
            var context = GetAbility<DataContext>();
            await context.AssignIdentity2Device(admin, device, identity);
        }
        public void DoReleaseDeviceFromIdentity(Device device, Identity identity) 
        {
            Search(identity.FirstName);
            var detailpage = Perform(new OpenTheIdentityDetailPage());
            detailpage.WebDriver = Driver;
            detailpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            ExpectedLog = GenericLogLineCreator.ReleaseIdentityFromDeviceLogLine($"identity with name: {identity.Name}", 
                $"{device.Category.Category} with {device.AssetTag}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheReleaceDevicePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseDevicePage");
            page.Title.Should().BeEquivalentTo("Release device from identity", "Title should be correct");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
        }
        public void DoAssignDevice2Identity(Device device, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine($"Identity with name: {identity.Name}",
                $"{device.Category.Category} with {device.AssetTag}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheAssignDevicePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignDevicePage");
            page.ClickDevice(device);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedDevice");
        }
        public void FillInAssignForm()
        {
            var page = OpenAssignFom();
            page.ITEmployee.Should().Be(admin.Account.UserID);
            Perform(new ClickTheGeneratePDFOnAssignForm());
        }
    }
}
