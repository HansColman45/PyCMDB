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
        
        Helpers.Laptop laptop;
        Laptop Laptop;
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
    }
}
