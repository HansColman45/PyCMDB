using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System;
using Xunit;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public sealed class LaptopSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private LaptopOverviewPage overviewPage;
        private CreateLaptopPage CreateLaptop;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.Laptop laptop;
        string expectedlog, updatedField, newValue;
        public LaptopSteps(ScenarioData scenarioData) : base(scenarioData)
        {

        }
        [Given(@"I want to create a new Laptop with these details")]
        public void GivenIWantToCreateANewLaptopWithTheseDetails(Table table)
        {
            laptop = table.CreateInstance<helpers.Laptop>();
            rndNr = rnd.Next();
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.LaptopOverview();
            entity.AssetCategory category = context.GetAssetCategory("Laptop");
            string Vendor, Type, assetType;
            assetType = laptop.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            CreateLaptop = overviewPage.New();
            CreateLaptop.AssetTag = laptop.AssetTag + rndNr.ToString();
            CreateLaptop.SerialNumber = laptop.SerialNumber + rndNr.ToString();
            CreateLaptop.RAM = laptop.RAM;
            CreateLaptop.Type = AssetType.TypeID.ToString();
        }
        [When(@"I save the Laptop")]
        public void WhenISaveTheLaptop()
        {
            CreateLaptop.Create();
        }
        [Then(@"I can find the newly created Laptop back")]
        public void ThenICanFindTheNewlyCreatedLaptopBack()
        {
            expectedlog = $"The Laptop with type {laptop.Type} is created by {admin.Account.UserID} in table laptop";
            overviewPage.Search(laptop.AssetTag + rndNr.ToString());
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            Assert.Equal(log, expectedlog);
        }

        [Given(@"There is an Laptop existing")]
        public void GivenThereIsAnLaptopExisting()
        {
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.LaptopOverview();
            overviewPage.Search("IND");
        }
        [When(@"I update the (.*) with (.*) and I save")]
        public void WhenIUpdateTheSerialnumberWithAndISave(string field, string value)
        {
            rndNr = rnd.Next();
            var updatepage = overviewPage.Update();
            laptop = new()
            {
                AssetTag = updatepage.AssetTag,
                SerialNumber = updatepage.SerialNumber,
                Type = updatepage.Type,
                RAM = updatepage.RAM
            };
            
            updatedField = field;
            switch (field)
            {
                case "Serialnumber":
                    newValue = value + rndNr.ToString();
                    updatepage.SerialNumber = newValue;
                    break;
                case "RAM":
                    newValue = value;
                    updatepage.RAM = newValue;
                    break;
            }
            updatepage.Edit();
        }
        [Then(@"The Laptop is saved")]
        public void ThenTheLaptopIsSaved()
        {
            overviewPage.Search(laptop.AssetTag);
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            switch (updatedField)
            {
                case "Serialnumber":
                    expectedlog = $"The SerialNumber in table laptop has been changed from {laptop.SerialNumber} to {newValue} by {admin.Account.UserID}";
                    break;
                case "RAM":
                    expectedlog = $"The RAM number in table laptop has been changed from {laptop.RAM} to {newValue} by {admin.Account.UserID}";
                    break;
            }
            Assert.Equal(log, expectedlog);
        }

        [Given(@"There is an active Laptop existing")]
        public void GivenThereIsAnActiveLaptopExisting()
        {
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.LaptopOverview();
            overviewPage.Search("IND");
        }
        [When(@"I deactivate the Laptop with reason (.*)")]
        public void WhenIDeactivateTheLaptopWithReasonTest(string reason)
        {
            newValue = reason;
            var deactivatepage = overviewPage.Deactivate();
            deactivatepage.Reason = reason;
            deactivatepage.Delete();
        }
        [Then(@"The laptop is deactivated")]
        public void ThenTheLaptopIsDeactivated()
        {
            overviewPage.Search("IND");
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            expectedlog = $"";
        }

        [Given(@"There is an inactive Laptop existing")]
        public void GivenThereIsAnInactiveLaptopExisting()
        {
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
            main = login.LogIn();
            overviewPage = main.LaptopOverview();
            overviewPage.Search("IND");
        }
        [When(@"I activate the Laptop")]
        public void WhenIActivateTheLaptop()
        {
        }
        [Then(@"The laptop is active")]
        public void ThenTheLaptopIsActive()
        {
        }

    }
}
