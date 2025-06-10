using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Actors.IdentityActors;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Tasks;
using Reqnroll;
using Identity = CMDB.Domain.Entities.Identity;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class IdentityStepDefinitions : TestBase
    {
        private IdentityCreator identityCreator;
        private IdentityUpdator identityUpdator;
        private IdentityDeviceActor identityDeviceActor;
        private IdentityAccountActor identityAccountActor;
        private IdentityMobileActor identityMobileActor;
        private IdentitySubscriptionActor subscriptionActor;

        private Helpers.Identity iden;
        private Identity Identity;
        private Device _Device;
        private Account Account;
        private Mobile Mobile;
        private Subscription Subscription;
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
        #region Release device
        [Given(@"The (.*) is assigned to the Identity")]
        public async Task GivenTheDeviceIsAssignedToTheIdentity(string device)
        {
            log.Debug($"Will assign the {device} to the Identity");
            await identityDeviceActor.AssignDevice2Identity(_Device, Identity);
        }
        [When(@"I release the (.*) from the Identity")]
        public void WhenIReleaseTheDeviceFromTheIdentity(string device)
        {
            log.Debug($"We will release the {device} from the Identity {Identity.Name}");
            identityDeviceActor.DoReleaseDeviceFromIdentity(_Device, Identity);
        }
        [When(@"I fill in the release form for my Identity")]
        public void WhenIFillInTheReleaseFormForMyIdentity()
        {
            identityDeviceActor.Perform<ClickTheGeneratePDFOnReleaseForm>();
        }
        [Then(@"The (.*) is released from the Identity")]
        public void ThenTheDeviceIsReleasedFromTheIdentity(string device)
        {
            log.Debug($"Will check if the {device} is released to the Identity");
            identityDeviceActor.Search(Identity.UserID);
            var lastLog = identityDeviceActor.IdentityLastLogLine;
            identityDeviceActor.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Assign and release account
        [Given(@"There is an active Identity existing in the system")]
        public async Task GivenThereIsAnActiveIdentityExistingInTheSystem()
        {
            identityAccountActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(identityAccountActor);
            Admin = await identityAccountActor.CreateNewAdmin();
            Identity = await identityAccountActor.CreateNewIdentity();
            identityAccountActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityAccountActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            identityAccountActor.OpenIdentityOverviewPage();
            identityAccountActor.Search(Identity.UserID);
        }
        [Given(@"an Account as well")]
        public async Task GivenAnAccountAsWell()
        {
            Account = await identityAccountActor.CreateAccount();
        }

        [When(@"I assign the Account to my Identity")]
        public void WhenIAssignTheAccountToMyIdentity()
        {
            identityAccountActor.AssignIdentity2Account(Identity, Account);
        }
        [When(@"I fill in the assignform")]
        public void WhenIFillInTheAssignform()
        {
            identityAccountActor.FillInAssignForm(Identity);
        }
        [Then(@"The Account is assigned to my Identity")]
        public void ThenTheAccountIsAssignedToMyIdentity()
        {
            identityAccountActor.Search(Identity.UserID);
            var lastLogLine = identityAccountActor.IdentityLastLogLine;
            identityAccountActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }

        [Given(@"The Identity is assigned to the account as well")]
        public async Task GivenTheIdentityIsAssignedToTheAccountAsWell()
        {
            await identityAccountActor.AssignAccount(Identity, Account);
        }
        [When(@"I release the Identity from the account")]
        public void WhenIReleaseTheIdentityFromTheAccount()
        {
            identityAccountActor.ReleaseAccount(Identity, Account);
        }
        [Then(@"The Identity is released from the account")]
        public void ThenTheIdentityIsReleasedFromTheAccount()
        {
            identityAccountActor.Search(Identity.UserID);
            var lastLogLine = identityAccountActor.IdentityLastLogLine;
            identityAccountActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
        #region Assign Mobile
        [Given(@"There is an Identity existing in the system")]
        public async Task TheIdentityIsExisiting()
        {
            identityMobileActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(identityMobileActor);
            Admin = await identityMobileActor.CreateNewAdmin();
            Identity = await identityMobileActor.CreateNewIdentity();
            identityMobileActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = identityMobileActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            identityMobileActor.OpenIdentityOverviewPage();
            identityMobileActor.Search(Identity.UserID);
        }
        
        [Given(@"The mobile is exisiting as well")]
        public async Task TheMobileIsExisting()
        {
            Mobile = await identityMobileActor.CreateNewMobile();
        }
        [When(@"I assign the mobile to the Identity")]
        public void IAssignTheMobile()
        {
            identityMobileActor.AssignMobile2Identity(Identity,Mobile);
        }
        [When(@"I fill in the assignform for my identity")]
        public void IFillInTheAssignForm()
        {
            identityMobileActor.FillInAssignForm(Identity);
        }
        [Then(@"The mobile is assigned to the Identity")]
        public void TheMobileIsAssignedToIdentity()
        {
            identityMobileActor.Search(Identity.UserID);
            string lastLogLine = identityMobileActor.IdentityLastLogLine;
            identityMobileActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
        #region Release mobile
        [Given(@"The mobile is assigned to my identity")]
        public async Task GivenTheMobileIsAssignedToMyIdentity()
        {
            await identityMobileActor.AssignMobile(Identity,Mobile);
        }

        [When(@"I relase the mobile from my identity")]
        public void WhenIRelaseTheMobileFromMyIdentity()
        {
            identityMobileActor.ReleaseMobile(Identity, Mobile);
        }

        [Then(@"The mobile is released")]
        public void ThenTheMobileIsReleased()
        {
            identityMobileActor.Search(Identity.UserID);
            string lastLogLine = identityMobileActor.IdentityLastLogLine;
            identityMobileActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
        
        [Given(@"There is Identity in the system")]
        public async Task GivenThereIsIdentityInTheSystem()
        {
            subscriptionActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(subscriptionActor);
            Admin = await subscriptionActor.CreateNewAdmin();
            Identity= await subscriptionActor.CreateNewIdentity();
            subscriptionActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = subscriptionActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            subscriptionActor.OpenIdentityOverviewPage();
            subscriptionActor.Search(Identity.UserID);
        }
        [Given(@"The Internet subscription exists as well")]
        public async Task GivenTheInternetSubscriptionExistsAsWell()
        {
            Subscription = await subscriptionActor.CreateInternetSubscription();
        }
        #region Assign Subscription
        [When(@"I assign the subscription")]
        public void WhenIAssignTheSubscription()
        {
            subscriptionActor.AssignSubscription(Identity,Subscription);
        }
        [When(@"I fill in the assignform to assign the subscription to my identity")]
        public void WhenIFillInTheAssignformToAssignTheSubscriptionToMyIdentity()
        {
            subscriptionActor.FillInAssignForm();
        }
        [Then(@"The subscription is assigned to my Identity")]
        public void ThenTheSubscriptionIsAssignedToMyIdentity()
        {
            subscriptionActor.Search(Identity.UserID);
            string lastLogLine = subscriptionActor.IdentityLastLogLine;
            subscriptionActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
        #region Release sub
        [Given(@"The subscription is assigned to my Identity")]
        public async Task GivenTheSubscriptionIsAssignedToMyIdentity()
        {
            await subscriptionActor.AssignSubscription2Identity(Identity,Subscription);
        }
        [When(@"I release the subscription")]
        public void WhenIReleaseTheSubscription()
        {
            subscriptionActor.ReleaseSubscription(Identity,Subscription);
        }
        [Then(@"The subsription is released from my Identity")]
        public void ThenTheSubsriptionIsReleasedFromMyIdentity()
        {
            subscriptionActor.Search(Identity.UserID);
            string lastLogLine = subscriptionActor.IdentityLastLogLine;
            subscriptionActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
    }
}
