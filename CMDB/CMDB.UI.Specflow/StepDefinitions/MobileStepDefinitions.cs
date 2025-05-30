using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Mobiles;
using CMDB.UI.Specflow.Questions;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class MobileStepDefinitions : TestBase
    {
        private MobileCreator mobileCreator;
        private MobileUpdator mobileUpdator;
        private MobileIdentityActor mobileIdentityActor;
        private MobileSubscriptionActor mobileSubscriptionActor;

        private Helpers.Mobile mobile;
        private Mobile Mobile;
        private Identity Identity;
        private Subscription subscription;

        public MobileStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }
        #region Create Mobile
        [Given(@"I want to create a new Mobile with these details")]
        public async Task GivenIWantToCreateANewMobileWithTheseDetails(Table table)
        {
            mobile = table.CreateInstance<Helpers.Mobile>();
            mobileCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(mobileCreator);
            Admin = await mobileCreator.CreateNewAdmin();
            mobileCreator.DoLogin(Admin.Account.UserID, "1234");
            bool result = mobileCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            mobileCreator.OpenMobileOverviewPage();
        }
        [When(@"I save the mobile")]
        public async Task WhenISaveTheMobile()
        {
            await mobileCreator.CreateNewMobile(mobile);
        }
        [Then(@"I can find the newly created Mobile back")]
        public void ThenICanFindTheNewlyCreatedMobileBack()
        {
            mobileCreator.SearchMobile(mobile);
            expectedlog = mobileCreator.GetLastMobileLogLine;
            mobileCreator.ExpectedLog.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        [Given(@"There is a mobile existing in the system")]
        public async Task GivenThereIsAMobileExistingInTheSystem()
        {
            mobileUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(mobileUpdator);
            Admin = await mobileUpdator.CreateNewAdmin();
            Mobile = await mobileUpdator.CreateMobile();
            log.Info($"Mobile created with IMEI {Mobile.IMEI}");
            mobileUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = mobileUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            mobileUpdator.OpenMobileOverviewPage();
            mobileUpdator.Search(Mobile.IMEI.ToString());
        }
        #region Update Mobile
        [When(@"I change the (.*) to (.*) and I save the changes for my mobile")]
        public async Task WhenIChangeTheIMEIToAndISaveTheChangesForMyMobile(string field, string value)
        {
            Mobile = await mobileUpdator.UpdateMobile(Mobile, field, value);
        }
        [Then(@"The changes in mobile are saved")]
        public void ThenTheChangesInMobileAreSaved()
        {
            mobileUpdator.Search(Mobile.IMEI.ToString());
            string lastLogLine = mobileUpdator.GetLastMobileLogLine;
            lastLogLine.Should().BeEquivalentTo(mobileUpdator.ExpectedLog);
        }
        #endregion
        #region Deactivate Mobile
        [When(@"I deactivate the mobile with reason (.*)")]
        public void WhenIDeactivateTheMobileWithReasonTest(string reason)
        {
            mobileUpdator.DeactivateMoble(Mobile, reason);
        }
        [Then(@"The mobile is deactivated")]
        public void ThenTheMobileIsDeactivated()
        {
            mobileUpdator.Search(Mobile.IMEI.ToString());
            string lastLogLine = mobileUpdator.GetLastMobileLogLine;
            lastLogLine.Should().BeEquivalentTo(mobileUpdator.ExpectedLog);
        }
        #endregion
        #region Activate Mobile
        [Given(@"There is an inactive mobile existing in the system")]
        public async Task GivenThereIsAnInactiveMobileExistingInTheSystem()
        {
            mobileUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(mobileUpdator);
            Admin = await mobileUpdator.CreateNewAdmin();
            Mobile = await mobileUpdator.CreateMobile(false);
            log.Info($"Mobile created with IMEI {Mobile.IMEI}");
            mobileUpdator.DoLogin(Admin.Account.UserID, "1234");
            bool result = mobileUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            mobileUpdator.OpenMobileOverviewPage();
            mobileUpdator.Search(Mobile.IMEI.ToString());
        }
        [When(@"I activate the mobile")]
        public void WhenIActivateTheMobile()
        {
            mobileUpdator.ActivateMobile(Mobile);
        }
        [Then(@"The mobile is actice again")]
        public void ThenTheMobileIsActiceAgain()
        {
            mobileUpdator.Search(Mobile.IMEI.ToString());
            string lastLogLine = mobileUpdator.GetLastMobileLogLine;
            lastLogLine.Should().BeEquivalentTo(mobileUpdator.ExpectedLog);
        }
        #endregion
        #region Assign and Relkease Iden
        [Given(@"There is an active mobile in the system")]
        public async Task GivenThereIsAnActiveMobileInTheSystem()
        {
            mobileIdentityActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(mobileIdentityActor);
            Admin = await mobileIdentityActor.CreateNewAdmin();
            Mobile = await mobileIdentityActor.CreateMobile();
            mobileIdentityActor.DoLogin(Admin.Account.UserID,"1234");
            bool resilt = mobileIdentityActor.Perform(new IsTheUserLoggedIn());
            resilt.Should().BeTrue();
            mobileIdentityActor.OpenMobileOverviewPage();
            mobileIdentityActor.Search(Mobile.IMEI.ToString());
        }

        [Given(@"The Identity to assign to my mobile exists as well")]
        public async Task GivenTheIdentityToAssignToMyMobileExistsAsWell()
        {
            Identity = await mobileIdentityActor.CreateNewIdentity();
        }

        [When(@"I want to assign that Identity to my mobile")]
        public void WhenIWantToAssignThatIdentityToMyMobile()
        {
            mobileIdentityActor.AssignMobile2Identity(Mobile,Identity);
        }
        [When(@"I fill in thee assign form")]
        public void WhenIFillInTheeAssignForm()
        {
            mobileIdentityActor.FillInAssignForm(Identity);
        }
        [Then(@"The identity is assigned to my mobile")]
        public void ThenTheIdentityIsAssignedToMyMobile()
        {
            mobileIdentityActor.Search(Mobile.IMEI.ToString());
            string lastLogLine = mobileIdentityActor.GetLastMobileLogLine;
            mobileIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }

        [Given(@"The Identity is assigned to my mobile")]
        public async Task GivenTheIdentityIsAssignedToMyMobile()
        {
            await mobileIdentityActor.AssignIdentity(Mobile,Identity);
        }
        [When(@"I release the Identity from my mobile")]
        public void WhenIReleaseTheIdentityFromMyMobile()
        {
            mobileIdentityActor.ReleaseIdentity(Mobile, Identity);
        }
        [Then(@"The identity is released from my mobile")]
        public void ThenTheIdentityIsReleasedFromMyMobile()
        {
            mobileIdentityActor.Search(Mobile.IMEI.ToString());
            string lastLogLine = mobileIdentityActor.GetLastMobileLogLine;
            mobileIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
        #region assign subscription
        [Given(@"There is an mobile in the system")]
        public async Task GivenThereIsAnMobileInTheSystem()
        {
            mobileSubscriptionActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(mobileSubscriptionActor);
            Admin = await mobileSubscriptionActor.CreateNewAdmin();
            Mobile = await mobileSubscriptionActor.CreateMobile();
            mobileSubscriptionActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = mobileSubscriptionActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            mobileSubscriptionActor.OpenMobileOverviewPage();
            mobileSubscriptionActor.Search(Mobile.IMEI.ToString());
        }
        [Given(@"An mobile subscription exist as well")]
        public async Task GivenAnMobileSubscriptionExistAsWell()
        {
            subscription = await mobileSubscriptionActor.CreateNewSubscription();
        }

        [When(@"I assign the mobile subscription to my mobile")]
        public void WhenIAssignTheMobileSubscriptionToMyMobile()
        {
            mobileSubscriptionActor.AssignSubscription2Mobile(Mobile, subscription);
        }
        [When(@"I fill in the assign form for this subscription")]
        public void WhenIFillInTheAssignFormForThisSubscription()
        {
            mobileSubscriptionActor.FillInAssignForm();
        }
        [Then(@"The subscription is assigned to my mobile")]
        public void ThenTheSubscriptionIsAssignedToMyMobile()
        {
            mobileSubscriptionActor.Search(Mobile.IMEI.ToString());
            string lastlog = mobileSubscriptionActor.GetLastMobileLogLine;
            mobileSubscriptionActor.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
        #region release sub
        [Given(@"The subscription is assigned to my mobile")]
        public async Task GivenTheSubscriptionIsAssignedToMyMobile()
        {
            await mobileSubscriptionActor.AssignSubscription(Mobile, subscription);
        }
        [When(@"I release the subscription from my mobile")]
        public void WhenIReleaseTheSubscriptionFromMyMobile()
        {
            mobileSubscriptionActor.ReleaseSubscription(Mobile, subscription);
        }
        [Then(@"The subscription is released")]
        public void ThenTheSubscriptionIsReleased()
        {
            mobileSubscriptionActor.Search(Mobile.IMEI.ToString());
            string lastlog = mobileSubscriptionActor.GetLastMobileLogLine;
            mobileSubscriptionActor.ExpectedLog.Should().BeEquivalentTo(lastlog);
        }
        #endregion
    }
}
