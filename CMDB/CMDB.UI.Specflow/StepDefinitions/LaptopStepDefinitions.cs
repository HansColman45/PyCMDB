using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Actors.Laptops;
using CMDB.UI.Specflow.Questions;
using TechTalk.SpecFlow.Assist;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class LaptopStepDefinitions : TestBase
    {
        LaptopCreator laptopCreator;
        LaptopUpdator laptopUpdator;
        LaptopIdentityActor laptopIdentityActor;
        
        Helpers.Laptop laptop;
        Laptop Laptop;
        Identity Identity;
        public LaptopStepDefinitions(ScenarioContext scenarioContext, ActorRegistry actorRegistry) : base(scenarioContext, actorRegistry)
        {
        }

        [Given(@"I want to create a new Laptop with these details")]
        public async Task GivenIWantToCreateANewLaptopWithTheseDetails(Table table)
        {
            laptopCreator = new(ScenarioContext);
            ActorRegistry.RegisterActor(laptopCreator);
            laptop = table.CreateInstance<Helpers.Laptop>();
            Admin = await laptopCreator.CreateNewAdmin();
            laptopCreator.DoLogin(Admin.Account.UserID, "1234");
            var result = laptopCreator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            laptopCreator.OpenLaptopOverviewPage();
        }
        [When(@"I save the Laptop")]
        public void WhenISaveTheLaptop()
        {
            laptopCreator.CreateNewLaptop(laptop);
        }
        [Then(@"I can find the newly created Laptop back")]
        public void ThenICanFindTheNewlyCreatedLaptopBack()
        {
            laptopCreator.SearchLaptop(laptop);
            var lastLog = laptopCreator.LaptopLastLogLine;
            laptopCreator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        
        [Given(@"There is an Laptop existing")]
        public async Task GivenThereIsAnLaptopExisting()
        {
            laptopUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(laptopUpdator);
            Admin = await laptopUpdator.CreateNewAdmin();
            Laptop = await laptopUpdator.CreateLaptop();
            laptopUpdator.DoLogin(Admin.Account.UserID, "1234");
            var result = laptopUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            laptopUpdator.OpenLaptopOverviewPage();
            laptopUpdator.Search(Laptop.AssetTag);
        }
        #region update Laptop
        [When(@"I update the (.*) with (.*) on my Laptop and I save")]
        public void WhenIUpdateTheSerialnumberWithOnMyLaptopAndISave(string field, string value)
        {
            Laptop = laptopUpdator.UpdateLaptop(Laptop, field, value);
        }
        [Then(@"The Laptop is saved")]
        public void ThenTheLaptopIsSaved()
        {
            laptopUpdator.Search(Laptop.AssetTag);
            var lastLog = laptopUpdator.LaptopLastLogLine;
            laptopUpdator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Deactivate Laptop
        [When(@"I deactivate the Laptop with reason (.*)")]
        public void WhenIDeactivateTheLaptopWithReasonTest(string reason)
        {
            laptopUpdator.DeactivateLaptop(Laptop, reason);
        }
        [Then(@"The laptop is deactivated")]
        public void ThenTheLaptopIsDeactivated()
        {
            laptopUpdator.Search(Laptop.AssetTag);
            var lastLog = laptopUpdator.LaptopLastLogLine;
            laptopUpdator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Activate Laptop
        [Given(@"There is an inactive Laptop existing")]
        public async Task GivenThereIsAnInactiveLaptopExisting()
        {
            laptopUpdator = new(ScenarioContext);
            ActorRegistry.RegisterActor(laptopUpdator);
            Admin = await laptopUpdator.CreateNewAdmin();
            Laptop = await laptopUpdator.CreateLaptop(false);
            laptopUpdator.DoLogin(Admin.Account.UserID, "1234");
            var result = laptopUpdator.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            laptopUpdator.OpenLaptopOverviewPage();
            laptopUpdator.Search(Laptop.AssetTag);
        }
        [When(@"I activate the Laptop")]
        public void WhenIActivateTheLaptop()
        {
            laptopUpdator.ActivateLaptop(Laptop);
        }
        [Then(@"The laptop is active")]
        public void ThenTheLaptopIsActive()
        {
            laptopUpdator.Search(Laptop.AssetTag);
            var lastLog = laptopUpdator.LaptopLastLogLine;
            laptopUpdator.ExpectedLog.Should().BeEquivalentTo(lastLog);
        }
        #endregion
        #region Assign and release
        [Given(@"There is an active Laptop existing")]
        public async Task GivenThereIsAnActiveLaptopExisting()
        {
            laptopIdentityActor = new(ScenarioContext);
            ActorRegistry.RegisterActor(laptopIdentityActor);
            Laptop = await laptopIdentityActor.CreateLaptop();
            Admin = await laptopIdentityActor.CreateNewAdmin();
            laptopIdentityActor.DoLogin(Admin.Account.UserID, "1234");
            bool result = laptopIdentityActor.Perform(new IsTheUserLoggedIn());
            result.Should().BeTrue();
            laptopIdentityActor.OpenLaptopOverviewPage();
            laptopIdentityActor.Search(Laptop.AssetTag);
        }
        [Given(@"The Identity to assign to my laptop is existing")]
        public async Task GivenTheIdentityToAssignToMyLaptopIsExisting()
        {
            Identity = await laptopIdentityActor.CreateNewIdentity();
        }

        [When(@"I assign the Laptop to the Identity")]
        public void WhenIAssignTheLaptopToTheIdentity()
        {
            laptopIdentityActor.AssignTheIdentity2Laptop(Laptop, Identity);
        }
        [When(@"I fill in the assign form for my Laptop")]
        public void WhenIFillInTheAssignFormForMyLaptop()
        {
            laptopIdentityActor.FillInAssignForm(Identity);
        }
        [Then(@"The Identity is assigned to the Laptop")]
        public void ThenTheIdentityIsAssignedToTheLaptop()
        {
            laptopIdentityActor.Search(Laptop.AssetTag);
            var lastLogLine = laptopIdentityActor.LaptopLastLogLine;
            laptopIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }

        [Given(@"that Identity is assigned to my Laptop")]
        public async Task GivenThatIdentityIsAssignedToMyLaptop()
        {
            await laptopIdentityActor.AssignIdentity(Laptop, Identity);
        }
        [When(@"I release that identity from my Laptop and I fill in the release form")]
        public void WhenIReleaseThatIdentityFromMyLaptopAndIFillInTheReleaseForm()
        {
            laptopIdentityActor.ReleaseIdentity(Laptop,Identity);
        }
        [Then(@"The identity is released from my Laptop")]
        public void ThenTheIdentityIsReleasedFromMyLaptop()
        {
            laptopIdentityActor.Search(Laptop.AssetTag);
            var lastLogLine = laptopIdentityActor.LaptopLastLogLine;
            laptopIdentityActor.ExpectedLog.Should().BeEquivalentTo(lastLogLine);
        }
        #endregion
    }
}
