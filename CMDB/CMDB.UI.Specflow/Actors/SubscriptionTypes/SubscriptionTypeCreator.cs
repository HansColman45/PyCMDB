using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;
using CMDB.UI.Specflow.Helpers;
using CMDB.UI.Specflow.Questions.SubscriptionType;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.SubscriptionTypes
{
    public class SubscriptionTypeCreator : SubscriptionTypeActor
    {
        public SubscriptionTypeCreator(ScenarioContext scenarioContext, string name = "SubscriptionTypeCreator") : base(scenarioContext, name)
        {
        }

        public void CreateType(SubscriptionType subscriptionType)
        {
            rndNr = rnd.Next();
            var page = Perform(new OpenTheSubscriptionTypeCreatePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_createPage");
            page.Category = subscriptionType.Category;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Category");
            page.Provider = subscriptionType.Provider + rndNr;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Provider");
            page.Type = subscriptionType.Type + rndNr;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            page.Description = subscriptionType.Description + rndNr;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Description");
            page.Create();
        }
        public void SearchType(SubscriptionType subscriptionType) 
        {
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"{subscriptionType.Category} with {subscriptionType.Provider}{rndNr} and {subscriptionType.Type}{rndNr}", admin.Account.UserID, Table);
            var page = GetAbility<SubscriptionTypeOverviewPage>();
            page.Search(subscriptionType.Type + rndNr);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
    }
}
