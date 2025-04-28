using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Subscription;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.Subscriptions
{
    public class SubscriptionIdentityAndMobileActor : SubscriptionUpdator
    {
        public SubscriptionIdentityAndMobileActor(ScenarioContext scenarioContext, string name = "SubscriptionIdentityAndMobileActor") : base(scenarioContext, name)
        {
        }
        public async Task<Identity> CreateIdentity()
        {
            return await Perform(new CreateTheIdentity());
        }
        public async Task<Mobile> CreateMobile()
        {
            return await Perform(new CreateTheMobile());
        }

        public async Task AssignIdentity2Subscription(Subscription subscription, Identity identity)
        {
            var context = GetAbility<DataContext>();
            await context.AssignIdentity2Subscription(admin, subscription, identity);
        }
        public async Task AssignMobile2Subscription(Subscription subscription, Mobile mobile)
        {
            var context = GetAbility<DataContext>();
            await context.AssignMobile2Subscription(admin, mobile, subscription);
        }
        public async Task AssignMobile2Identity(Mobile mobile, Identity identity)
        {
            var context = GetAbility<DataContext>();
            await context.AssignIdentity2Mobile(admin, mobile, identity);
        }

        public void DoAssignIdenity2Subscription(Subscription subscription, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine($"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}", 
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var page = Perform(new OpenTheSubscriptionAssignIdentityPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignIdentityPage");
            page.SelectIdentity(identity);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectSubscription");
        }
        public void DoAssignMobile2Sunscription(Subscription subscription, Mobile mobile)
        {
            string subscriptionInfo = $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            string mobileinfo = $"mobile with type {mobile.MobileType}";
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine(subscriptionInfo, mobileinfo, admin.Account.UserID, Table);
            var page = Perform(new OpenTheSubscriptionAssignMobilePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignMobilePage");
            page.SelectMobile(mobile);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_MobileSelected");
        }

        public void FillInAssignForm()
        {
            var page = OpenAssignFom();
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            Perform(new ClickTheGeneratePDFOnAssignForm());
        }

        public void DoReleaseIdenity(Subscription subscription, Identity identity)
        {
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine($"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}",
                $"Identity with name: {identity.Name}", admin.Account.UserID, Table);
            var detailpage = Perform(new OpenTheSubscriptionDetailPage());
            detailpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheSubscriptionReleasePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }

        internal void DoReleaseMobile(Subscription subscription, Mobile mobile, Identity identity)
        {
            string subscriptionInfo = $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            string mobileinfo = $"mobile with type {mobile.MobileType}";
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(subscriptionInfo, mobileinfo, admin.Account.UserID, Table);
            var detailpage = Perform(new OpenTheSubscriptionDetailPage());
            detailpage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheMobileSubscriptionReleasePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseIdentityPage");
            page.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            page.Employee.Should().BeEquivalentTo(identity.Name, "The employee should be the name of the identity");
            page.CreatePDF();
        }
    }
}
