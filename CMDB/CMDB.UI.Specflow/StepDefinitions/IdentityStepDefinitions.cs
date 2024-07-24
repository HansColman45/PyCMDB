using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Actors.IdentityActors;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;
using Identity = CMDB.Domain.Entities.Identity;
using Table = TechTalk.SpecFlow.Table;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class IdentityStepDefinitions: TestBase
    {
        private Helpers.Identity iden;
        private Identity Identity;
        private Device _Device;
        private IdentityCreator identityCreator;
        private IdentityUpdator identityUpdator;
        private IdentityDeviceActor identityDeviceActor;
        private CreateIdentityPage createIdentity;
        private string updatedfield;

        public IdentityStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }
        #region Create Identity
        [Given(@"I want to create an Identity with these details")]
        public async Task GivenIWantToCreateAnIdentityWithTheseDetails(Table table)
        {
            identityCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(identityCreator);
            iden = table.CreateInstance<Helpers.Identity>();
            Admin = await identityCreator.CreateNewAdmin();
            identityCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            identityCreator.OpenIdentityOverviewPage();
            createIdentity = identityCreator.OpenCreateIdentityPage();
            identityCreator.CreateNewIdentity(iden);
        }
        [When(@"I save")]
        public void WhenISave()
        {
            createIdentity.Create();
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Saved");
        }
        [Then(@"I can find the newly created Identity back")]
        public void ThenICanFindTheNewlyCreatedIdentityBack()
        {
            identityCreator.SearchIdentity(iden);
            var lastLog = identityCreator.IdentityLastLogLine;
            expectedlog = identityCreator.ExpectedLog;
            lastLog.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        #region Edit Identity
        [Given(@"An Identity exisist in the system")]
        public async Task GivenAnIdentityExisistInTheSystem()
        {
            identityUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(identityUpdator);
            Admin = await identityUpdator.CreateNewAdmin();
            identityUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            identityUpdator.OpenIdentityOverviewPage(); 
        }
        [When(@"I want to update (.*) with (.*)")]
        public async Task WhenIWantToUpdateFirstNameWithTestje(string field, string newValue)
        {
            updatedfield = field;
            Identity = await identityUpdator.CreateNewIdentity();
            log.Info($"Identity created with FirstName {Identity.FirstName} and LastName {Identity.LastName}");
            identityUpdator.Search(Identity.FirstName);
            identityUpdator.OpenUpdateIdentityPage();
            Identity = identityUpdator.UpdateIdentity(field, newValue, Identity);
        }
        [Then(@"The identity is updated")]
        public void ThenTheIdentityIsUpdated()
        {
            switch (updatedfield)
            {
                case "FirstName":
                    identityUpdator.Search(Identity.LastName);
                    break;
                case "LastName":
                case "Company":
                case "UserID":
                case "Email":
                    identityUpdator.Search(Identity.FirstName);
                    break;
            }
            var log = identityUpdator.IdentityLastLogLine;
            expectedlog = identityUpdator.ExpectedLog;
            log.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        #region Ideneity Actions
        [Given(@"An inactive Identity exisist in the system")]
        public async Task GivenAnInactiveIdentityExisistInTheSystem()
        {
            identityUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(identityUpdator);
            Admin = await identityUpdator.CreateNewAdmin();
            identityUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            identityUpdator.OpenIdentityOverviewPage();
            Identity = await identityUpdator.CreateNewIdentity(false);
            log.Info($"Identity created with FirstName {Identity.FirstName} and LastName {Identity.LastName}");
            identityUpdator.Search(Identity.FirstName);
        }
        [When(@"I want to activate this identity")]
        public void WhenIWantToActivateThisIdentity()
        {
            identityUpdator.Activate(Identity);
        }
        [Then(@"The Identity is active")]
        public void ThenTheIdentityIsActive()
        {
            identityUpdator.Search(Identity.FirstName);
            expectedlog = identityUpdator.ExpectedLog;
            var log = identityUpdator.IdentityLastLogLine;
            log.Should().BeEquivalentTo(expectedlog);
        }

        [Given(@"An acive Identity exisist in the system")]
        public async Task GivenAnAciveIdentityExisistInTheSystem()
        {
            identityUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(identityUpdator);
            Admin = await identityUpdator.CreateNewAdmin();
            identityUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            identityUpdator.OpenIdentityOverviewPage();
            Identity = await identityUpdator.CreateNewIdentity();
            log.Info($"Identity created with FirstName {Identity.FirstName} and LastName {Identity.LastName}");
            identityUpdator.Search(Identity.FirstName);
        }
        [When(@"I want to deactivete the identity whith the reason (.*)")]
        public void WhenIWantToDeactiveteTheIdentityWhithTheReasonTest(string reason)
        {
            identityUpdator.OpenDeactivateIdentityPage();
            identityUpdator.Deactivate(reason, Identity);
        }
        [Then(@"The Idenetity is inactive")]
        public void ThenTheIdenetityIsInactive()
        {
            identityUpdator.Search(Identity.FirstName);
            expectedlog = identityUpdator.ExpectedLog;
            var log = identityUpdator.IdentityLastLogLine;
            log.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        #region Assign Device
        [Given(@"An active Identity exisist in the system")]
        public async Task GivenAnActiveIdentityExisistInTheSystem()
        {
            identityDeviceActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(identityDeviceActor);
            Admin = await identityDeviceActor.CreateNewAdmin();
            Identity = await identityDeviceActor.CreateNewIdentity();
            identityDeviceActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityDeviceActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            identityDeviceActor.OpenIdentityOverviewPage();
            identityDeviceActor.Search(Identity.UserID);
        }
        [Given(@"a (.*) exist as well")]
        public async Task GivenALaptopExistAsWell(string device)
        {
            _Device = await identityDeviceActor.CreateNewDevice(device);
        }
        [When(@"I assign that (.*) to the identity")]
        public void WhenIAssignThatLaptopToTheIdentity(string device)
        {
            log.Debug($"Will try to assign the {device} to the Identity");
            identityDeviceActor.DoAssignDevice2Identity(_Device, Identity);
        }
        [When(@"I fill in the assig form")]
        public void WhenIFillInTheAssigForm()
        {
            identityDeviceActor.FillInAssignForm();
        }
        [Then(@"The (.*) is assigned")]
        public void ThenTheLaptopIsAssigned(string device)
        {
            log.Debug($"Will check if the {device} is assigned to the Identity");
            identityDeviceActor.Search(Identity.UserID);
            var lastLog = identityDeviceActor.IdentityLastLogLine;
            identityDeviceActor.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
    }
}
