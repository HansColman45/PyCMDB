using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.SubscriptionTypes;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class SubscriptionTypeStepDefinitions : TestBase
    {
        private Helpers.SubscriptionType subscriptionType;
        private SubscriptionType SubscriptionType;

        private SubscriptionTypeCreator SubscriptionTypeCreator;
        private SubscriptionTypeUpdator SubscriptionTypeUpdator;
        public SubscriptionTypeStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }

        [Given(@"I want to create a Subscriptiontype with the folowing details")]
        public async Task IWantToCreateToken(Table table)
        {
            subscriptionType = table.CreateInstance<Helpers.SubscriptionType>();
            SubscriptionTypeCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(SubscriptionTypeCreator);
            Admin = await SubscriptionTypeCreator.CreateNewAdmin();
            SubscriptionTypeCreator.DoLogin(Admin.Account.UserID, "1234");
            var result = SubscriptionTypeCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
        }
        [When(@"I save the subscriptiontype")]
        public void ISaveSubscription()
        {
            SubscriptionTypeCreator.CreateType(subscriptionType);
        }
        [Then(@"I can find the newly create subscriptiontype back")]
        public void ICanFindTheNewBack() 
        {
            SubscriptionTypeCreator.SearchType(subscriptionType);
            var lasLogLine = SubscriptionTypeCreator.LastLogLine;
            SubscriptionTypeCreator.ExpectedLog.Should().BeEquivalentTo(lasLogLine);
        }
        
        [Given(@"There is a subscription type existing")]
        public async Task ThereIsATypeExisting()
        {
            SubscriptionTypeUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(SubscriptionTypeUpdator);
            SubscriptionType = await SubscriptionTypeUpdator.CreateNewSubscriptionType();
            Admin = await SubscriptionTypeUpdator.CreateNewAdmin();
            SubscriptionTypeUpdator.DoLogin(Admin.Account.UserID, "1234");
            var result = SubscriptionTypeUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            SubscriptionTypeUpdator.OpenSubscriptionTypeOverviewPage();
            SubscriptionTypeUpdator.Search(SubscriptionType.Type);
        }
        #region Edit Type
        [When(@"I update the (.*) and change it to (.*) and save the subscriptiontype")]
        public void WhenIUpdateTheSubscriptionType(string field, string value)
        {
            SubscriptionType = SubscriptionTypeUpdator.EditType(SubscriptionType, field, value);
        }
        [Then(@"I can see the changes in the subscription type")]
        public void ICandSeeTheUpdatedType()
        {
            SubscriptionTypeUpdator.Search(SubscriptionType.Type);
            var lasLogLine = SubscriptionTypeUpdator.LastLogLine;
            SubscriptionTypeUpdator.ExpectedLog.Should().BeEquivalentTo(lasLogLine);
        }
        #endregion
        #region Deactivate
        [When(@"I deactivate the subscriptiontype with (.*)")]
        public void IdeactivateTheTokenWithAReason(string reason)
        {
            SubscriptionTypeUpdator.DeactivateType(SubscriptionType, reason);
        }
        [Then(@"The subscriptiontype is deactivated")]
        public void ThenTheSubscriptiontypeIsDeactivated()
        {
            SubscriptionTypeUpdator.Search(SubscriptionType.Type);
            var lasLogLine = SubscriptionTypeUpdator.LastLogLine;
            SubscriptionTypeUpdator.ExpectedLog.Should().BeEquivalentTo(lasLogLine);
        }
        #endregion
        #region Activate an inactive Type
        [Given(@"There is an inactive subscriptiontype existing")]
        public async void ThereIsAnInactiveType()
        {
            SubscriptionTypeUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(SubscriptionTypeUpdator);
            Admin = await SubscriptionTypeUpdator.CreateNewAdmin();
            SubscriptionType = await SubscriptionTypeUpdator.CreateNewSubscriptionType(false);            
            SubscriptionTypeUpdator.DoLogin(Admin.Account.UserID, "1234");
            var result = SubscriptionTypeUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            SubscriptionTypeUpdator.OpenSubscriptionTypeOverviewPage();
            SubscriptionTypeUpdator.Search(SubscriptionType.Type);
        }
        [When(@"I activate the subscriptiontype")]
        public void WhenIActivateTheSubscriptionType()
        {
            SubscriptionTypeUpdator.ActivateType(SubscriptionType);
        }
        [Then(@"The subscriptiontype is activated")]
        public void TheTypeIsActivated()
        {
            SubscriptionTypeUpdator.Search(SubscriptionType.Type);
            var lasLogLine = SubscriptionTypeUpdator.LastLogLine;
            SubscriptionTypeUpdator.ExpectedLog.Should().BeEquivalentTo(lasLogLine);
        }
        #endregion
    }
}
