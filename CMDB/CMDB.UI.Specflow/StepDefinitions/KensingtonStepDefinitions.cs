using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Kensingtons;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class KensingtonStepDefinitions: TestBase
    {
        KensingtonCreator kensingtonCreator;
        KensingtonUpdator kensingtonUpdator;
        Helpers.Kensington kensington;
        Kensington Kensington;
        public KensingtonStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }
        #region create
        [Given(@"I have a Kensington with these detials")]
        public async Task GivenIHaveAKensingtonWithTheseDetials(Table table)
        {
            kensington = table.CreateInstance<Helpers.Kensington>();
            kensingtonCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(kensingtonCreator);
            Admin = await kensingtonCreator.CreateNewAdmin();
            kensingtonCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = kensingtonCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            kensingtonCreator.OpenKensingtonOverviewPage();
        }
        [When(@"I save the Kensington")]
        public async Task WhenISaveTheKensington()
        {
            await kensingtonCreator.CreateKensington(kensington);
        }
        [Then(@"I can find the newly create Kensington back")]
        public void ThenICanFindTheNewlyCreateKensingtonBack()
        {
            kensingtonCreator.Search(kensington);
            string lastlog = kensingtonCreator.LastLogLine;
            kensingtonCreator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region update
        [Given(@"There is a Kensington existing in the system")]
        public async Task GivenThereIsAnKensingtonExistingInTheSystem()
        {
            kensingtonUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(kensingtonUpdator);
            Admin = await kensingtonUpdator.CreateNewAdmin();
            Kensington = await kensingtonUpdator.CreateKensington();
            kensingtonUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = kensingtonUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            kensingtonUpdator.OpenKensingtonOverviewPage();
            kensingtonUpdator.Search(Kensington.SerialNumber);
        }
        [When(@"I update the (.*) and change it to (.*) and save the Kensington")]
        public void WhenIUpdateTheSerialNumberAndChangeItToAndSaveTheKensington(string field, string newValue)
        {
            Kensington = kensingtonUpdator.Update(Kensington, field, newValue);
        }
        [Then(@"I can find the updated Kensington back")]
        public void ThenICanFindTheUpdatedKensingtonBack()
        {
            kensingtonUpdator.Search(Kensington.SerialNumber);
            string lastlog = kensingtonUpdator.LastLogLine;
            kensingtonUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        [When(@"I deactivate the Kensington with the reason (.*)")]
        public void WhenIDeactivateTheKensingtonWithTheReasonTest(string reason)
        {
            kensingtonUpdator.DeactivateKensington(Kensington, reason);
        }
        [Then(@"The Kensington is deactivated")]
        public void ThenTheKensingtonIsDeactivated()
        {
            kensingtonUpdator.Search(Kensington.SerialNumber);
            string lastlog = kensingtonUpdator.LastLogLine;
            kensingtonUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #region activate
        [Given(@"There is an inactive Kensington existing in the system")]
        public async Task GivenThereIsAnInactiveKensingtonExistingInTheSystem()
        {
            kensingtonUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(kensingtonUpdator);
            Admin = await kensingtonUpdator.CreateNewAdmin();
            Kensington = await kensingtonUpdator.CreateKensington(false);
            kensingtonUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = kensingtonUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            kensingtonUpdator.OpenKensingtonOverviewPage();
            kensingtonUpdator.Search(Kensington.SerialNumber);
        }
        [When(@"I activate the Kensington")]
        public void WhenIActivateTheKensington()
        {
            kensingtonUpdator.ActivateKensington(Kensington);
        }
        [Then(@"The Kensington is activated")]
        public void ThenTheKensingtonIsActivated()
        {
            kensingtonUpdator.Search(Kensington.SerialNumber);
            string lastlog = kensingtonUpdator.LastLogLine;
            kensingtonUpdator.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
    }
}
