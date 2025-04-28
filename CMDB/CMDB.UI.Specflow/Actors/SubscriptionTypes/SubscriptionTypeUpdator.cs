
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.SubscriptionType;

namespace CMDB.UI.Specflow.Actors.SubscriptionTypes
{
    public class SubscriptionTypeUpdator : SubscriptionTypeActor
    {
        public SubscriptionTypeUpdator(ScenarioContext scenarioContext, string name = "SubscriptionTypeUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<SubscriptionType> CreateNewSubscriptionType(bool active = true)
        {
            try
            {
                if (active)
                    return await Perform(new CreateTheSubscriptionType());
                else
                    return await Perform(new CreateTheInactiveSubscriptionType());
            }
            catch (Exception e)
            {
                log.Fatal(e);
                throw;
            }
        }

        public SubscriptionType EditType(SubscriptionType subscriptionType,string field, string value)
        {
            rndNr = rnd.Next();
            var page = Perform(new OpenTheSubscriptionTypeEditPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_editPage");
            switch (field) 
            {
                case "provider":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, subscriptionType.Provider, value + rndNr, admin.Account.UserID, Table);
                    page.Provider = value + rndNr;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Provider");
                    subscriptionType.Provider = value + rndNr;
                    page.Edit();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_updated");
                    break;
                case "type":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, subscriptionType.Type,value+rndNr,admin.Account.UserID,Table);
                    page.Type = value + rndNr;
                    subscriptionType.Type = value + rndNr;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
                    page.Edit();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_updated");
                    break;
                case "description":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field,subscriptionType.Description,value+rndNr, admin.Account.UserID, Table);
                    page.Description = value + rndNr;
                    subscriptionType.Description = value + rndNr;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Description");
                    page.Edit();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_updated");
                    break;
            }
            return subscriptionType;
        }
        public void DeactivateType(SubscriptionType subscriptionType, string reason)
        {
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"{subscriptionType.Category.Category} with {subscriptionType.Provider} and {subscriptionType.Type}", admin.Account.UserID,reason,Table);
            var page = Perform(new OpenTheSubscriptionTypeDeactivatePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_reason");
            page.Delete();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deleted");
        }
        public void ActivateType(SubscriptionType subscriptionType) 
        {
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"{subscriptionType.Category.Category} with {subscriptionType.Provider} and {subscriptionType.Type}", admin.Account.UserID, Table);
            var page = GetAbility<SubscriptionTypeOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
    }
}
