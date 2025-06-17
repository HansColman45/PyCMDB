
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Questions.Keys;
using CMDB.UI.Specflow.Questions.Laptop;
using CMDB.UI.Specflow.Questions.Main;
using CMDB.UI.Specflow.Tasks;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Laptops
{
    internal class LaptopIdentityActor : LaptopUpdator
    {
        public LaptopIdentityActor(ScenarioContext scenarioContext, string name = "LaptopIdentityActor") : base(scenarioContext, name)
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
        public async Task AssignIdentity(Laptop laptop, Identity identity)
        {
            try
            {
                var context = GetAbility<DataContext>();
                await context.AssignIdentity2Device(admin, laptop, identity);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }
        }
        public async Task AssignKey(Laptop laptop, Kensington kensington)
        {
            var context = GetAbility<DataContext>();
            await context.AssignDevice2Key(admin, laptop, kensington);
        }
        public void AssignTheIdentity2Laptop(Laptop laptop, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine($"Laptop with {laptop.AssetTag}",
               $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheLaptopAssignIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignIdentityPage");
            page.SelectIdentity(identity);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedIdentity");
        }
        public void FillInAssignForm(Identity identity)
        {
            if(ScenarioContext.ScenarioInfo.Title.Contains("assign an existing Identiy to my Laptop")) { 
                var assignForm = OpenAssignFom();
                assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
                assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
                Perform<ClickTheGeneratePDFOnAssignForm>();
                assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
            }
            else
            {
                var assignForm = Perform(new OpenTheKensingtonAssignFormPage());
                assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignForm");
                assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
                assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
                assignForm.CreatePDF();
                assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
            }
        }
        public void ReleaseIdentity(Laptop laptop,Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine($"Laptop with {laptop.AssetTag}",
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheLaptopDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheLaptopReleaseIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
        public void AssignTheKey2Laptop(Laptop laptop, Kensington key)
        {
            Search(laptop.AssetTag);
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{laptop.Category.Category} with {laptop.AssetTag}";
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine(deviceinfo, keyinfo, admin.Account.UserID, Table);
            var page = Perform(new OpenTheLaptopAssignKensingtonPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignKeyPage");
            page.SelectKensington(key);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedKey");
        }
        public void ReleaseTheKeyFromLaptop(Laptop laptop, Kensington key, Identity identity) 
        { 
            Search(laptop.AssetTag);
            string keyinfo = $"Kensington with serial number: {key.SerialNumber}";
            string deviceinfo = $"{laptop.Category.Category} with {laptop.AssetTag}";
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(deviceinfo, keyinfo, admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheLaptopDetailPage());
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheLaptopReleaseKensingtonPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseKensingtonPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
    }
}
