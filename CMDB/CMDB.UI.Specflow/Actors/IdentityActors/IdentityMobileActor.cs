
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.IdentityActors
{
    public class IdentityMobileActor : IdentityUpdator
    {
        public IdentityMobileActor(ScenarioContext scenarioContext, string name = "IdentityMobileActor") : base(scenarioContext, name)
        {
        }

        public async Task<Mobile> CreateNewMobile()
        {
            return await Perform(new CreateTheMobile());
        }
        public async Task AssignMobile(Identity identity, Mobile mobile)
        {
            var context = GetAbility<DataContext>();
            await context.AssignIdentity2Mobile(admin, mobile, identity);
        }

        public void AssignMobile2Identity(Identity identity, Mobile mobile)
        {
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine($"Identity with name: {identity.Name}", 
                $"mobile with type {mobile.MobileType}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheAssignDevicePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignDevicePage");
            page.ClickMobile(mobile);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedMobile");
        }
        public void FillInAssignForm(Identity identity)
        {
            var assignForm = OpenAssignFom();
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            Perform<ClickTheGeneratePDFOnAssignForm>();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
        }

        public void ReleaseMobile(Identity identity, Mobile mobile)
        {
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine($"Identity with name: {identity.Name}",
                $"mobile with type {mobile.MobileType}", admin.Account.UserID, Table);
            Search(identity.FirstName);
            var detailpage = Perform(new OpenTheIdentityDetailPage());
            detailpage.WebDriver = Driver;
            detailpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheReleaseMobilePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseDevicePage");
            page.Title.Should().BeEquivalentTo("Release mobile from identity", "Title should be correct");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            Perform(new ClickTheGeneratePDFOnReleaseMobileForm());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Released");
        }
    }
}
