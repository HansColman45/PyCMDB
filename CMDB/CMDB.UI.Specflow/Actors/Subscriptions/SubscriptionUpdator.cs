using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Subscription;

namespace CMDB.UI.Specflow.Actors.Subscriptions
{
    public class SubscriptionUpdator : SubscriptionActor
    {
        public SubscriptionUpdator(ScenarioContext scenarioContext, string name = "SubscriptionUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Subscription> CreateNewMobileSubscription(bool active = true)
        {
            if (active)
                return await Perform(new CreateTheMobileSubscription());
            else
                return await Perform(new CreateTheInactiveMobileSubscription());
        }
        public async Task<Subscription> CreateNewInternetSubscription(bool active = true)
        {
            if (active)
                return await Perform(new CreateTheInternetSubscription());
            else
                return await Perform(new CreateTheInactiveInternetSubscription());
        }
        public Subscription Update(Subscription subscription)
        {
            rndNr = rnd.Next();
            ExpectedLog = GenericLogLineCreator.UpdateLogLine("phone number", subscription.PhoneNumber, rndNr.ToString(), admin.Account.UserID, Table);
            var editPage = Perform(new OpenTheSubscriptionEditPage());
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
            editPage.Phonenumber = rndNr.ToString();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PhoneNumber");
            editPage.Edit();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Edited");
            subscription.PhoneNumber = rndNr.ToString();
            return subscription;
        }

        public void Deactivate(Subscription subscription, string reason)
        {
            string value = $"Subscription with Category: {subscription.Category.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            ExpectedLog = GenericLogLineCreator.DeleteLogLine(value, admin.Account.UserID, reason, Table);
            var page = Perform(new OpenTheSubscriptionDeactivatePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivate");
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            page.Delete();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        public void Activate(Subscription subscription) 
        {
            string value = $"Subscription with Category: {subscription.Category.Category} and type {subscription.SubscriptionType} on {subscription.PhoneNumber}";
            ExpectedLog = GenericLogLineCreator.ActivateLogLine(value,admin.Account.UserID,Table);
            var page = GetAbility<SubscriptionOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
    }
}
