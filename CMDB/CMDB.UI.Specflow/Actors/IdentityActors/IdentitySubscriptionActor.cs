
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.IdentityActors
{
    public class IdentitySubscriptionActor : IdentityUpdator
    {
        public IdentitySubscriptionActor(ScenarioContext scenarioContext, string name = "IdentitySubscriptionActor") : base(scenarioContext, name)
        {
        }

        public async Task<Subscription> CreateInternetSubscription()
        {
            return await Perform(new CreateTheInternetSubscription());
        }
        public async Task AssignSubscription2Identity(Identity identity, Subscription subscription)
        {
            var context = GetAbility<DataContext>();
            await context.AssignIdentity2Subscription(admin,subscription,identity);
        }

        public void AssignSubscription(Identity identity, Subscription subscription)
        {
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine($"Identity with name: {identity.Name}",
                $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheAssignDevicePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignDevicePage");
            page.ClickSubscription(subscription);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectSubscription");
        }
        public void FillInAssignForm()
        {
            var page = OpenAssignFom();
            page.ITEmployee.Should().Be(admin.Account.UserID);
            Perform(new ClickTheGeneratePDFOnAssignForm());
        }

        public void ReleaseSubscription(Identity identity, Subscription subscription)
        {
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine($"Identity with name: {identity.Name}",
                $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}", admin.Account.UserID, Table);
            Search(identity.FirstName);
            var detailpage = Perform(new OpenTheIdentityDetailPage());
            detailpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheReleaseSubscriptionPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseSubscriptionPage");
            page.Title.Should().BeEquivalentTo("Release subscription from identity", "Title should be correct");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            Perform(new ClickTheGeneratePDFOnReleaseSubscriptionForm());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Released");
        }
    }
}
