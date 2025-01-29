using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Mobile;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.Mobiles
{
    public class MobileSubscriptionActor : MobileUpdator
    {
        public MobileSubscriptionActor(ScenarioContext scenarioContext, string name = "MobileSubscriptionActor") : base(scenarioContext, name)
        {
        }
        public async Task<Subscription> CreateNewSubscription()
        {
            return await Perform(new CreateTheMobileSubscription());
        }
        public async Task AssignSubscription(Mobile mobile, Subscription subscription)
        {
            var context = GetAbility<DataContext>();
            await context.AssignMobile2Subscription(admin,mobile, subscription);
        }

        public void AssignSubscription2Mobile(Mobile mobile, Subscription subscription) 
        {
            string subscriptionInfo = $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            string mobileinfo = $"mobile with type {mobile.MobileType}";
            ExpectedLog = GenericLogLineCreator.AssingDevice2IdenityLogLine(mobileinfo, subscriptionInfo, admin.Account.UserID, Table);
            var page = Perform(new OpenTheMobileAssignSubscriptionPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignSubscriptionPage");
            page.SelectSubscription(subscription);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectSubscription");
        }
        public void FillInAssignForm()
        {
            var assignForm = OpenAssignFom();
            Perform<ClickTheGeneratePDFOnAssignForm>();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
        }

        public void ReleaseSubscription(Mobile mobile, Subscription subscription)
        {
            string subscriptionInfo = $"Subscription: {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            string mobileinfo = $"mobile with type {mobile.MobileType}";
            ExpectedLog = GenericLogLineCreator.ReleaseDeviceFromIdentityLogLine(mobileinfo,subscriptionInfo,admin.Account.UserID, Table);
            var detailPage = Perform(new OpenTheMobileDetailPage());
            detailPage.WebDriver = Driver;
            detailPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DetailPage");
            var page = Perform(new OpenTheMobileReleaseSubscriptionPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ReleaseSubscriptionPage");
            page.CreatePDF();
        }
    }
}
