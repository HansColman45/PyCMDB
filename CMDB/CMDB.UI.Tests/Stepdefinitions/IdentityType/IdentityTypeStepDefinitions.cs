using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System;
using Xunit;
using System.Threading.Tasks;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Pages;
using FluentAssertions;
using CMDB.UI.Tests.Hooks;
using CMDB.Testing.Helpers;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public class IdentityTypeStepDefinitions: TestBase
    {
        private LoginPage login;
        private MainPage main;
        private TypeOverviewPage overviewPage;
        private CreateTypePage createPage;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.IdentiyType identiyType;
        private entity.IdentityType IdentityType;
        private string expectedLog;
        public IdentityTypeStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"I want to create an Identity type with these details")]
        public void GivenIWantToCreateAnIdentityTypeWithTheseDetails(Table table)
        {
            identiyType = table.CreateInstance<helpers.IdentiyType>();
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.IdentityTypeOverview();
            overviewPage = main.IdentityTypeOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            createPage = overviewPage.New();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            createPage.Type = identiyType.Type + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetType");
            createPage.Description = identiyType.Description + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SetDescription");
        }
        [When(@"I save the Identity type")]
        public void WhenISaveTheIdentityType()
        {
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }

        [Then(@"The I can find the newly create Identity type back")]
        public void ThenTheICanFindTheNewlyCreateIdentityTypeBack()
        {
            expectedLog = $"The Identitytype created with type: {identiyType.Type + rndNr.ToString()} and description: {identiyType.Description + rndNr.ToString()} in table identitytype by {admin.Account.UserID}";
            overviewPage.Search(identiyType.Type + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedLog,"Log should match");
        }
    }
}
