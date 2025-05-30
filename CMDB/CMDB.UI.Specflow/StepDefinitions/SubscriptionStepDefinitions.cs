using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Subscriptions;
using CMDB.UI.Specflow.Questions;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class SubscriptionStepDefinitions: TestBase
    {
        SubscriptionCreator CreatorActor;
        SubscriptionUpdator SubscriptionUpdator;
        SubscriptionIdentityAndMobileActor SubscriptionIdentityAndMobileActor;

        Helpers.Subscription Subscription;
        Subscription subscription;
        Identity Identity;
        Mobile Mobile;

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
        [Given(@"There is an internet subscription existing in the system")]
        public async Task GivenThereIsAnInternetSubscriptionExistingInTheSystem()
        {
            SubscriptionIdentityAndMobileActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(SubscriptionIdentityAndMobileActor);
            Admin = await SubscriptionIdentityAndMobileActor.CreateNewAdmin();
            subscription = await SubscriptionIdentityAndMobileActor.CreateNewInternetSubscription();
            SubscriptionIdentityAndMobileActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = SubscriptionIdentityAndMobileActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            SubscriptionIdentityAndMobileActor.OpenSubscriptionOverviewPage();
            SubscriptionIdentityAndMobileActor.Search(subscription.PhoneNumber);
        }
        [Given(@"an identity exist as well")]
        public async Task GivenAnIdentityExistAsWell()
        {
            Identity = await SubscriptionIdentityAndMobileActor.CreateIdentity();
        }
        #region Assign identity
        [When(@"I assign the identity to the subscription")]
        public void WhenIAssignTheIdentityToTheSubscription()
        {
            SubscriptionIdentityAndMobileActor.DoAssignIdenity2Subscription(subscription, Identity);
        }
        [When(@"I fill in the assign form for my subscription")]
        public void WhenIFillInTheAssignFormForMySubscription()
        {
            SubscriptionIdentityAndMobileActor.FillInAssignForm();
        }
        [Then(@"The subscription is assigned to the identity")]
        public void ThenTheSubscriptionIsAssignedToTheIdentity()
        {
            SubscriptionIdentityAndMobileActor.Search(subscription.PhoneNumber);
            var lastlogLine = SubscriptionIdentityAndMobileActor.LastLogLine;
            SubscriptionIdentityAndMobileActor.ExpectedLog.Should().BeEquivalentTo(lastlogLine);
        }
        #endregion
        #region release iden
        [Given(@"The internet subsciption is assigend to my subscription")]
        public async Task GivenTheInternetSubsciptionIsAssigendToMySubscription()
        {
            await SubscriptionIdentityAndMobileActor.AssignIdentity2Subscription(subscription, Identity);
        }
        [When(@"I release the identity from my subscription")]
        public void WhenIReleaseTheIdentityFromMySubscription()
        {
            SubscriptionIdentityAndMobileActor.DoReleaseIdenity(subscription, Identity);
        }
        [Then(@"The Identity is released from the subscription")]
        public void ThenTheIdentityIsReleasedFromTheSubscription()
        {
            SubscriptionIdentityAndMobileActor.Search(subscription.PhoneNumber);
            var lastlogLine = SubscriptionIdentityAndMobileActor.LastLogLine;
            SubscriptionIdentityAndMobileActor.ExpectedLog.Should().BeEquivalentTo(lastlogLine);
        }
        #endregion
        [Given(@"There is a mobile subscription existing in the system")]
        public async Task GivenThereIsAMobileSubscripotionExistingInTheSystem()
        {
            SubscriptionIdentityAndMobileActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(SubscriptionIdentityAndMobileActor);
            Admin = await SubscriptionIdentityAndMobileActor.CreateNewAdmin();
            subscription = await SubscriptionIdentityAndMobileActor.CreateNewMobileSubscription();
            SubscriptionIdentityAndMobileActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = SubscriptionIdentityAndMobileActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            SubscriptionIdentityAndMobileActor.OpenSubscriptionOverviewPage();
            SubscriptionIdentityAndMobileActor.Search(subscription.PhoneNumber);
        }
        [Given(@"There is a mobile existing in the system as well")]
        public async Task GivenThereIsAMobileExistingInTheSystemAsWell()
        {
            Mobile = await SubscriptionIdentityAndMobileActor.CreateMobile();
        }
        [Given(@"The mobile is asssigned to an idenity")]
        public async Task GivenTheMobileIsAsssignedToAnIdenity()
        {
            Identity = await SubscriptionIdentityAndMobileActor.CreateIdentity();
            await SubscriptionIdentityAndMobileActor.AssignMobile2Identity(Mobile, Identity);
        }
        #region Assign mobile
        [When(@"I assign the subscription to my mobile")]
        public void WhenIAssignTheSubscriptionToMyMobile()
        {
            SubscriptionIdentityAndMobileActor.DoAssignMobile2Sunscription(subscription,Mobile);
        }
        [Then(@"The mobile is assigned to my subscription")]
        public void ThenTheMobileIsAssignedToMySubscription()
        {
            SubscriptionIdentityAndMobileActor.Search(subscription.PhoneNumber);
            var lastlogLine = SubscriptionIdentityAndMobileActor.LastLogLine;
            SubscriptionIdentityAndMobileActor.ExpectedLog.Should().BeEquivalentTo(lastlogLine);
        }
        #endregion
        #region release sub
        [Given(@"The mobile is assigned to my subscription")]
        public async Task GivenTheMobileIsAssignedToMySubscription()
        {
            await SubscriptionIdentityAndMobileActor.AssignMobile2Subscription(subscription, Mobile);
        }
        [When(@"I release the subcription from my mobile")]
        public void WhenIReleaseTheSubcriptionFromMyMobile()
        {
            SubscriptionIdentityAndMobileActor.DoReleaseMobile(subscription, Mobile, Identity);
        }
        [Then(@"The mobile is released from the subscription")]
        public void ThenTheMobileIsReleasedFromTheSubscription()
        {
            SubscriptionIdentityAndMobileActor.Search(subscription.PhoneNumber);
            var lastlogLine = SubscriptionIdentityAndMobileActor.LastLogLine;
            SubscriptionIdentityAndMobileActor.ExpectedLog.Should().BeEquivalentTo(lastlogLine);
        }
        #endregion
    }
}
