using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Tokens;
using CMDB.UI.Specflow.Questions;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class TokenStepDefinitions: TestBase
    {
        private TokenCreator tokenCreator;
        private TokenUpdator tokenUpdator;
        private TokenIdentityActor tokenIdentityActor;

        private Helpers.Token token;
        private Token Token;
        private Identity Identity;
        public TokenStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }

        [Given(@"I want to create a new Token with these details")]
        public async Task GivenIWantToCreateANewTokenWithTheseDetails(Table table)
        {
            tokenCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(tokenCreator);
            Admin = await tokenCreator.CreateNewAdmin();
            token = table.CreateInstance<Helpers.Token>();
            tokenCreator.DoLogin(Admin.Account.UserID, "1234");
            var result = tokenCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            tokenCreator.OpenTokenOverviewPage();
        }
        [When(@"I save the Token")]
        public async Task WhenISaveTheToken()
        {
            await tokenCreator.CreateNewToken(token);
        }
        [Then(@"I can find the newly created Token back")]
        public void ThenICanFindTheNewlyCreatedTokenBack()
        {
            tokenCreator.SearchToken(token);
            var lastLogLine = tokenCreator.LastLogLine;
            tokenCreator.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        
        [Given(@"There is an active Token existing")]
        public async Task GivenThereIsAnActiveTokenExisting()
        {
            tokenUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(tokenUpdator);
            Admin = await tokenUpdator.CreateNewAdmin();
            Token = await tokenUpdator.CreateNewToken();
            tokenUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = tokenUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            tokenUpdator.OpenTokenOverviewPage();
            tokenUpdator.Search(Token.AssetTag);
        }
        #region Update
        [When(@"I update the (.*) and change it to (.*) and save my Token")]
        public async Task WhenIUpdateTheSerialNumberAndChangeItToAndSaveMyToken(string field, string newValue)
        {
            await tokenUpdator.EditToken(Token, field, newValue);
        }
        [Then(@"I can see the changes")]
        public void ThenICanSeeTheChanges()
        {
            tokenUpdator.Search(Token.AssetTag);
            var lastLogline = tokenUpdator.LastLogLine;
            tokenUpdator.ExpectedLog.Should().BeEquivalentTo(lastLogline);
        }

        [When(@"I delete the token with reason (.*)")]
        public void WhenIDeleteTheTokenWithReasonTest(string reason)
        {
            tokenUpdator.DeactivateToken(Token, reason);
        }

        [Then(@"The token is deleted")]
        public void ThenTheTokenIsDeleted()
        {
            tokenUpdator.Search(Token.AssetTag);
            var lastLogline = tokenUpdator.LastLogLine;
            tokenUpdator.ExpectedLog.Should().BeEquivalentTo(lastLogline);
        }
        #endregion
        #region activate
        [Given(@"There is an inactive token existing")]
        public async Task GivenThereIsAnInactiveTokenExisting()
        {
            tokenUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(tokenUpdator);
            Admin = await tokenUpdator.CreateNewAdmin();
            Token = await tokenUpdator.CreateNewToken(false);
            tokenUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = tokenUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            tokenUpdator.OpenTokenOverviewPage();
            tokenUpdator.Search(Token.AssetTag);
        }
        [When(@"I activate that token")]
        public void WhenIActivateThatToken()
        {
            tokenUpdator.ActivateToken(Token);
        }
        [Then(@"The token is active")]
        public void ThenTheTokenIsActive()
        {
            tokenUpdator.Search(Token.AssetTag);
            var lastLogline = tokenUpdator.LastLogLine;
            tokenUpdator.ExpectedLog.Should().BeEquivalentTo(lastLogline);
        }
        #endregion
        #region Assign and Release identity
        [Given(@"There is an token existing in the system")]
        public async Task GivenThereIsAnTokenExistingInTheSystem()
        {
            tokenIdentityActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(tokenIdentityActor);
            Admin = await tokenIdentityActor.CreateNewAdmin();
            Token = await tokenIdentityActor.CreateNewToken();
            tokenIdentityActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = tokenIdentityActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            tokenIdentityActor.OpenTokenOverviewPage();
            tokenIdentityActor.Search(Token.AssetTag);
        }
        [Given(@"The Identity to assign to the token exists as well")]
        public async Task GivenTheIdentityToAssignToTheTokenExistsAsWell()
        {
            Identity = await tokenIdentityActor.CreateNewIdentity();
        }

        [When(@"I assign the Identity to the token")]
        public void WhenIAssignTheIdentityToTheToken()
        {
            tokenIdentityActor.AssignTheIdentity2Token(Token,Identity);
        }
        [When(@"I fill in the assignform for the token")]
        public void WhenIFillInTheAssignformForTheToken()
        {
            tokenIdentityActor.FillInAssignForm(Identity);
        }
        [Then(@"The Identity is assigned to my token")]
        public void ThenTheIdentityIsAssignedToMyToken()
        {
            tokenIdentityActor.Search(Token.AssetTag);
            string lastLogLine = tokenIdentityActor.LastLogLine;
            tokenIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }

        [Given(@"The Identity is assigned to the token")]
        public async Task GivenTheIdentityIsAssignedToTheToken()
        {
            await tokenIdentityActor.AssignIdentity(Token,Identity);
        }
        [When(@"I release the Identity")]
        public void WhenIReleaseTheIdentity()
        {
            tokenIdentityActor.ReleaseIdentity(Token, Identity);
        }
        [Then(@"The Identity is released from the token")]
        public void ThenTheIdentityIsReleasedFromTheToken()
        {
            tokenIdentityActor.Search(Token.AssetTag);
            string lastLogLine = tokenIdentityActor.LastLogLine;
            tokenIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
    }
}
