using CMDB.UI.Specflow.Actors.Subscriptions;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class SubscriptionStepDefinitions: TestBase
    {
        SubscriptionCreator CreatorActor;

        Helpers.Subscription Subscription;

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
    }
}
