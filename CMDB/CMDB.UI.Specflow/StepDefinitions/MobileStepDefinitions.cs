using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Mobiles;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class MobileStepDefinitions : TestBase
    {
        private MobileCreator mobileCreator;
        private MobileUpdator mobileUpdator;

        private Helpers.Mobile mobile;
        private Mobile Mobile;

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
    }
}
