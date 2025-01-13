using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Subscriptions;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class SubscriptionStepDefinitions: TestBase
    {
        SubscriptionCreator CreatorActor;
        SubscriptionUpdator SubscriptionUpdator;

        Helpers.Subscription Subscription;
        Subscription subscription;

        public SubscriptionStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }

        [Given(@"I want to create a Subscription with the following details")]
        public async Task GivenIWantToCreateASubscriptionWithTheFollowingDetails(Table table)
        {
            Subscription = table.CreateInstance<Helpers.Subscription>();
            CreatorActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(CreatorActor);
            Admin = await CreatorActor.CreateNewAdmin();
            CreatorActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = CreatorActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            CreatorActor.OpenSubscriptionOverviewPage();
        }
        [When(@"I save the subscription")]
        public async Task WhenISaveTheSubscription()
        {
            await CreatorActor.Create(Subscription);
        }
        [Then(@"I can find the newly create subscription back")]
        public void ThenICanFindTheNewlyCreateSubscriptionBack()
        {
            CreatorActor.Search(Subscription);
            string lastLogLine = CreatorActor.LastLogLine;
            CreatorActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        
        [Given(@"There is an subscription existing in the system")]
        public async Task GivenThereIsAnSubscriptionExistingInTheSystem()
        {
            SubscriptionUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(SubscriptionUpdator);
            Admin = await SubscriptionUpdator.CreateNewAdmin();
            subscription = await SubscriptionUpdator.CreateNewMobileSubscription();
            SubscriptionUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = SubscriptionUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            SubscriptionUpdator.OpenSubscriptionOverviewPage();
            SubscriptionUpdator.Search(subscription.PhoneNumber);
        }
        #region update
        [When(@"I update the phonenumber and save the changes")]
        public void WhenIUpdateThePhonenumberAndSaveTheChanges()
        {
            subscription = SubscriptionUpdator.Update(subscription);
        }
        [Then(@"I can see the changes in the subscription")]
        public void ThenICanSeeTheChangesInTheSubscription()
        {
            SubscriptionUpdator.Search(subscription.PhoneNumber);
            var lastLog = SubscriptionUpdator.LastLogLine;
            SubscriptionUpdator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Deactivate
        [When(@"I deactivate the subscription with (.*)")]
        public void WhenIDeactivateTheSubscriptionWithTest(string reason)
        {
            SubscriptionUpdator.Deactivate(subscription,reason);
        }
        [Then(@"The subscription is deactivated")]
        public void ThenTheSubscriptionIsDeactivated()
        {
            SubscriptionUpdator.Search(subscription.PhoneNumber);
            var lastLog = SubscriptionUpdator.LastLogLine;
            SubscriptionUpdator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Activate
        [Given(@"There is an inactive subscription in the system")]
        public async Task GivenThereIsAnInactiveSubscriptionInTheSystem()
        {
            SubscriptionUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(SubscriptionUpdator);
            Admin = await SubscriptionUpdator.CreateNewAdmin();
            subscription = await SubscriptionUpdator.CreateNewMobileSubscription(false);
            SubscriptionUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = SubscriptionUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            SubscriptionUpdator.OpenSubscriptionOverviewPage();
            SubscriptionUpdator.Search(subscription.PhoneNumber);
        }
        [When(@"I activate the subscription")]
        public void WhenIActivateTheSubscription()
        {
            SubscriptionUpdator.Activate(subscription);
        }
        [Then(@"The subscription is active")]
        public void ThenTheSubscriptionIsActive()
        {
            SubscriptionUpdator.Search(subscription.PhoneNumber);
            var lastLog = SubscriptionUpdator.LastLogLine;
            SubscriptionUpdator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
    }
}
