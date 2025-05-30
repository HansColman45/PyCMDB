using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Kensingtons;
using CMDB.UI.Specflow.Questions;
using System.Threading.Tasks;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class KensingtonStepDefinitions: TestBase
    {
        KensingtonCreator kensingtonCreator;
        KensingtonUpdator kensingtonUpdator;
        KensingtonDeviceActor kensingtonDeviceActor;

        Helpers.Kensington kensington;
        Kensington Kensington;
        Identity Identity;
        Device Device;
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
        #region Assign device
        [Given(@"There is an active Kensington existing in the system")]
        public async Task GivenThereIsAnActiveKensingtonExistingInTheSystem()
        {
            kensingtonDeviceActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(kensingtonDeviceActor);
            Admin = await kensingtonDeviceActor.CreateNewAdmin();
            Kensington = await kensingtonDeviceActor.CreateKensington();
            kensingtonDeviceActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = kensingtonDeviceActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            kensingtonDeviceActor.OpenKensingtonOverviewPage();
            kensingtonDeviceActor.Search(Kensington.SerialNumber);
        }

        [Given(@"a active (.*) existing in the system")]
        public async Task GivenThereIsAnLaptopExistingInTheSystem(string category)
        {
            Device = await kensingtonDeviceActor.CreateNewDevice(category);
        }
        [Given(@"That (.*) is assiged to an Identity")]
        public async Task GivenThatLaptopIsAssigedToAnIdentity(string category)
        {
            log.Info($"Assigning the device with category {category} to an identity");
            Identity = await kensingtonDeviceActor.CreateNewIdentity();
            await kensingtonDeviceActor.AssignDevice2Identity(Device, Identity);
        }
        [When(@"I link the Kensington to that (.*)")]
        public void WhenILinkTheKensingtonToThatLaptop(string category)
        {
            log.Info($"Assigning the device with category {category} to an kensington");
            kensingtonDeviceActor.DoAssignKey2Device(Kensington, Device);
        }
        [When(@"I fill in the assign form for that device")]
        public void WhenIFillInTheAssignFormForThatDevice()
        {
            kensingtonDeviceActor.FillInAssignForm(Identity);
        }
        [Then(@"The Kensington is linked to the device")]
        public void ThenTheKensingtonIsLinkedToTheDevice()
        {
            kensingtonDeviceActor.Search(Kensington.SerialNumber);
            string lastlog = kensingtonDeviceActor.LastLogLine;
            kensingtonDeviceActor.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region release device
        [Given(@"That (.*) is linked to my key")]
        public async Task GivenThatLaptopIsLinkedToMyKey(string category)
        {
            log.Info($"Assigning the device with category {category} to an kensington");
            await kensingtonDeviceActor.AssignDevice2Key(Device, Kensington);
        }
        [When(@"I release the (.*) from the Kensington")]
        public void WhenIReleaseTheLaptopFromTheKensington(string category)
        {
            log.Info($"Release the device with category {category} from my kensington");
            kensingtonDeviceActor.DoReleaseDevice(Kensington,Device,Identity);
        }
        [Then(@"The Kensington is released from the device")]
        public void ThenTheKensingtonIsReleasedFromTheDevice()
        {
            kensingtonDeviceActor.Search(Kensington.SerialNumber);
            string lastlog = kensingtonDeviceActor.LastLogLine;
            kensingtonDeviceActor.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
    }
}
