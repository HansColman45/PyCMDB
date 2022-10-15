using CMDB.UI.Tests.Pages;
using System;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using TechTalk.SpecFlow.Assist;
using CMDB.UI.Tests.Hooks;
using CMDB.Testing.Helpers;
using Bogus.DataSets;
using FluentAssertions;

namespace CMDB.UI.Tests.Stepdefinitions.SubscriptionTypes
{
    [Binding]
    public class CreateSubscriptionTypeStepDefinitions: TestBase
    {
        private LoginPage login;
        private MainPage main;
        private SubscriptionTypeOverviewPage overviewPage;
        private CreateSubscriptionTypePage createPage;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.SubscriptionType SubscriptionType;
        private entity.SubscriptionType subscriptionType;
        string expectedlog;

        public CreateSubscriptionTypeStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"I want to create a Subscriptiontype with the folowing details")]
        public void GivenIWantToCreateASubscriptiontypeWithTheFolowingDetails(Table table)
        {
            SubscriptionType = table.CreateInstance<helpers.SubscriptionType>();
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.SubscriptionTypeOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            createPage = overviewPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_New");
            createPage.Category = SubscriptionType.Category;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Category");
            createPage.Provider = SubscriptionType.Provider+rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Provider");
            createPage.Type = SubscriptionType.Type+rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            createPage.Description = SubscriptionType.Description+rndNr.ToString();
        }
        [When(@"I save the subscriptiontype")]
        public void WhenISaveTheSubscriptiontype()
        {
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"I can find the newly create subscriptiontype back")]
        public void ThenICanFindTheNewlyCreateSubscriptiontypeBack()
        {
            expectedlog = $"The {SubscriptionType.Category} with {SubscriptionType.Provider}{rndNr} and {SubscriptionType.Type}{rndNr} is created by {admin.Account.UserID} in table subscriptiontype";
            overviewPage.Search(SubscriptionType.Type + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }
    }
}
