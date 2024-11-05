using CMDB.UI.Specflow.Actors.Laptops;
using CMDB.UI.Specflow.Actors.Tokens;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class TokenStepDefinitions: TestBase
    {
        private TokenCreator tokenCreator;
        private Helpers.Token token;
        public TokenStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }

        [Given(@"I want to create a new Token with these details")]
        public async Task GivenIWantToCreateANewTokenWithTheseDetails(Table table)
        {
            tokenCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(tokenCreator);
            token = table.CreateInstance<Helpers.Token>();
            Admin = await tokenCreator.CreateNewAdmin();
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
    }
}
