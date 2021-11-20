using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace CMDB.UI.Tests.Stepdefinitions.AssetType
{
    [Binding]
    public sealed class AssetTypesSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private AssetTypeOverviewPage overviewPage;

        private readonly Random rnd = new();
        private int rndNr;
        private string vendor, type;
        public AssetTypesSteps(ScenarioData scenarioData) : base(scenarioData)
        {
        }
        [Given(@"I want to create a new (.*) with (.*) and (.*)")]
        public void GivenIWantToCreateANewKensingtonWithKensingtonAndBlack(string category, string vendor, string type)
        {
            rndNr = rnd.Next();
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.AssetTypeOverview();
            var newPage = overviewPage.New();
            newPage.Category = category;
            newPage.Vendor = vendor + rndNr.ToString();
            newPage.Type = type + rndNr.ToString();
            this.vendor = vendor + rndNr.ToString();
            this.type = type + rndNr.ToString();
        }
        [When(@"I create that (.*)")]
        public void WhenICreateThatKensington(string category)
        {
            overviewPage.Create();
        }

        [Then(@"The (.*) is created")]
        public void ThenTheIsCreated(string category)
        {
            overviewPage.Search(vendor);
            string expectedlog = $"The {category} type Vendor: {vendor} and type {type} is created by {admin.Account.UserID} in table assettype";
            var detail = overviewPage.Detail();
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }

    }
}
