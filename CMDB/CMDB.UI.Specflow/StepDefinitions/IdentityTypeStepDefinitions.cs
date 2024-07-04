using Bright.ScreenPlay.Actors;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.IdentityActors;
using CMDB.UI.Specflow.Actors.IdentityTypes;
using CMDB.UI.Specflow.Helpers;
using CMDB.UI.Specflow.Questions;
using Microsoft.Graph;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class IdentityTypeStepDefinitions : TestBase
    {
        IdentityTypeCreator identityTypeCreator;
        IdentityTypeUpdator IdentityTypeUpdator;
        Helpers.IdentiyType IdentityType;
        IdentityType identityType;

        public IdentityTypeStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }
        #region Create Identity type
        [Given(@"I want to create an Identity type with these details")]
        public async Task GivenIWantToCreateAnIdentityTypeWithTheseDetails(Table table)
        {
            IdentityType = table.CreateInstance<Helpers.IdentiyType>();
            identityTypeCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(identityTypeCreator);
            Admin = await identityTypeCreator.CreateNewAdmin();
            identityTypeCreator.DoLogin(Admin.Account.UserID,"1234");
            identityTypeCreator.OpenIdentityTypeOverviewPage();
        }
        [When(@"I save the Identity type")]
        public void WhenISaveTheIdentityType()
        {
            identityTypeCreator.CreateIdentityType(IdentityType);
        }
        [Then(@"The I can find the newly create Identity type back")]
        public void ThenTheICanFindTheNewlyCreateIdentityTypeBack()
        {
            identityTypeCreator.SearchIdentityType(IdentityType);
            var lastlog = identityTypeCreator.IdentityTypeLastLogLine;
            identityTypeCreator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        [Given(@"There is an Identity type existing")]
        public async Task GivenThereIsAnIdentityTypeExisting()
        {
            IdentityTypeUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(IdentityTypeUpdator);
            Admin = await IdentityTypeUpdator.CreateNewAdmin();
            identityType = await IdentityTypeUpdator.CreateNewIdentityType();
            IdentityTypeUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = IdentityTypeUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            IdentityTypeUpdator.OpenIdentityTypeOverviewPage();
            IdentityTypeUpdator.Search(identityType.Type);
        }
        #region Edit Identity type
        [When(@"I change the (.*) to (.*) and I save the Identity type")]
        public void WhenIChangeTheFieldToValueAndISaveTheIdentityType(string field, string value)
        {
            identityType = IdentityTypeUpdator.UpdateIdentity(identityType, field, value);
        }
        [Then(@"The Identity type is changed and the new values are visable")]
        public void ThenTheIdentityTypeIsChangedAndTheNewValuesAreVisable()
        {
            IdentityTypeUpdator.Search(identityType.Type);
            var lastlog = IdentityTypeUpdator.IdentityTypeLastLogLine;
            IdentityTypeUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region Deactivate Identity type
        [When(@"I want to deactivate the identity type with reason (.*)")]
        public void WhenIWantToDeactivateTheIdentityTypeWithReasonTest(string reason)
        {
            IdentityTypeUpdator.DeactiveIdentityType(identityType, reason);
        }
        [Then(@"The Identity type is deactivated")]
        public void ThenTheIdentityTypeIsDeactivated()
        {
            IdentityTypeUpdator.Search(identityType.Type);
            var lastlog = IdentityTypeUpdator.IdentityTypeLastLogLine;
            IdentityTypeUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region Activate Identity type
        [Given(@"There is an inactive Identitytype existing")]
        public async Task GivenThereIsAnInactiveIdentitytypeExisting()
        {
            IdentityTypeUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(IdentityTypeUpdator);
            Admin = await IdentityTypeUpdator.CreateNewAdmin();
            identityType = await IdentityTypeUpdator.CreateNewIdentityType(false);
            IdentityTypeUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = IdentityTypeUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            IdentityTypeUpdator.OpenIdentityTypeOverviewPage();
            IdentityTypeUpdator.Search(identityType.Type);
        }
        [When(@"I want to activate the Idenity type")]
        public void WhenIWantToActivateTheIdenityType()
        {
            IdentityTypeUpdator.ActiveIdentityType(identityType);
        }
        [Then(@"The Identity type is active")]
        public void ThenTheIdentityTypeIsActive()
        {
            IdentityTypeUpdator.Search(identityType.Type);
            var lastlog = IdentityTypeUpdator.IdentityTypeLastLogLine;
            IdentityTypeUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
    }
}
