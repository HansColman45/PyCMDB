using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;
using CMDB.UI.Specflow.Questions.Subscription;
using SubscriptionType = CMDB.Domain.Entities.SubscriptionType;

namespace CMDB.UI.Specflow.Actors.Subscriptions
{
    public class SubscriptionCreator : SubscriptionActor
    {
        public SubscriptionCreator(ScenarioContext scenarioContext, string name = "SubscriptionCreator") : base(scenarioContext, name)
        {
        }
        private async Task<SubscriptionType> GetOrCreateType(string type)
        {
            var context = GetAbility<DataContext>();
            return await context.GetOrCreateSubscriptionType(admin, type);
        }
        public async Task Create(Helpers.Subscription subscription)
        {
            rndNr = rnd.Next();
            var type = await GetOrCreateType(subscription.Type);
            var createPage = Perform(new OpenTheSubscriptionCreatePage());
            createPage.WebDriver = Driver;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            createPage.Type = type.Description;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedType");
            createPage.Phonenumber = subscription.PhoneNumber + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnteredPhoneNumber");
            ExpectedLog = $"The subscription with: {type.Category.Category} and type {type} on {subscription.PhoneNumber}{rndNr} is created by {admin.Account.UserID} in table subscription";
            createPage.Create();
        }
        public void Search(Helpers.Subscription subscription) 
        {
            var page = GetAbility<SubscriptionOverviewPage>();
            page.Search(subscription.PhoneNumber + rndNr.ToString());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
    }
}
