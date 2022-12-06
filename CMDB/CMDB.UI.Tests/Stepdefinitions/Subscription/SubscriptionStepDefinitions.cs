using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using TechTalk.SpecFlow.Assist;
using CMDB.Testing.Helpers;
using FluentAssertions;

namespace CMDB.UI.Tests.Stepdefinitions.Subscription
{
    [Binding]
    public class SubscriptionStepDefinitions : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private SubscriptionOverviewPage overviewPage;
        private CreateSubscriptionPage createPage;

        private readonly Random rnd = new();
        private int rndNr;

        helpers.Subscription _Subscription;
        entity.Subscription _subscription;
        string expectedlog;

        public SubscriptionStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }
        [Given(@"I want to create a Subscription with the following details")]
        public async void GivenIWantToCreateASubscriptionWithTheFollowingDetails(Table table)
        {
            _Subscription = table.CreateInstance<helpers.Subscription>();
            entity.SubscriptionType type= await context.GetOrCreateSubscriptionType(admin, _Subscription.Type);
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
            overviewPage = main.SubscriptionOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            createPage = overviewPage.New();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            createPage.Type = type.Description; 
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectedType");
            createPage.Phonenumber = _Subscription.PhoneNumber + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnteredPhoneNumber");
            expectedlog = $"The subscription with: {type.Category.Category} and {type} on {_Subscription.PhoneNumber}{rndNr} is created by {admin.Account.UserID} in table subscription"; 
        }
        [When(@"I save the subscription")]
        public void WhenISaveTheSubscription()
        {
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"I can find the newly create subscription back")]
        public void ThenICanFindTheNewlyCreateSubscriptionBack()
        {
            overviewPage.Search(monitor.AssetTag + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

    }
}
