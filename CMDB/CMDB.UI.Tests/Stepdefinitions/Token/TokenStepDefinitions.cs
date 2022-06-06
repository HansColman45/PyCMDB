using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using TechTalk.SpecFlow;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using TechTalk.SpecFlow.Assist;
using CMDB.Testing.Helpers;
using FluentAssertions;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public class TokenStepDefinitions: TestBase
    {
        private LoginPage login;
        private MainPage main;
        private TokenOverviewPage overviewPage;
        private CreateTokenPage createPage;
        private UpdateTokenPage updatePage;
        private TokenAssignIdentityPage AssignPage;
        private TokenReleaseIdentityPage releaseIdenity;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.Token token;
        private entity.Token Token;
        private entity.Identity Identity;
        string expectedlog, updatedField, newValue;

        public TokenStepDefinitions(ScenarioData scenarioData, ScenarioContext scenarioContext) : base(scenarioData, scenarioContext)
        {
        }

        [Given(@"I want to create a new Token with these details")]
        public void GivenIWantToCreateANewTokenWithTheseDetails(Table table)
        {
            token = table.CreateInstance<helpers.Token>();
            entity.AssetCategory category = context.GetAssetCategory("Token");
            string Vendor, Type, assetType;
            assetType = token.Type;
            Vendor = assetType.Split(" ")[0];
            Type = assetType.Split(" ")[1];
            entity.AssetType AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
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
            overviewPage = main.TokenOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            createPage = overviewPage.New();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Create");
            createPage.AssetTag = token.AssetTag + rndNr.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssetTag");
            createPage.Type = AssetType.TypeID.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            createPage.SerialNumber = token.SerialNumber;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
        }
        [When(@"I save the Token")]
        public void WhenISaveTheToken()
        {
            createPage.Create();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        [Then(@"I can find the newly created Token")]
        public void ThenICanFindTheNewlyCreatedToken()
        {
            expectedlog = $"The Token with type {token.Type} is created by {admin.Account.UserID} in table token";
            overviewPage.Search(token.AssetTag + rndNr.ToString());
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }

        [Given(@"There is an token existing")]
        public async Task GivenThereIsAnTokenExisting()
        {
            Token = await context.CreateToken(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.TokenOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(Token.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I update the (.*) with (.*) on my token and I save")]
        public void WhenIUpdateTheSerialNumberWithOnMyTokenAndISave(string field, string value)
        {
            updatePage = overviewPage.Update();
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Update");
            updatedField = field;
            string Vendor, Type, assetType;
            entity.AssetType AssetType = new();
            if (field == "Type")
            {
                entity.AssetCategory category = context.GetAssetCategory("Token");
                assetType = value;
                Vendor = assetType.Split(" ")[0];
                Type = assetType.Split(" ")[1];
                AssetType = context.GetOrCreateAssetType(Vendor, Type, category);
            }
            switch (field)
            {
                case "SerialNumber":
                    newValue = value + rndNr.ToString();
                    updatePage.SerialNumber = newValue;
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
                    break;
                case "Type":
                    newValue = AssetType.ToString();
                    updatePage.Type = AssetType.TypeID.ToString();
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_TypeUpdated");
                    break;
            }
            updatePage.Edit();
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_updated");
        }
        [Then(@"Then the token is saved with the new value")]
        public void ThenThenTheTokenIsSavedWithTheNewValue()
        {
            switch (updatedField)
            {
                case "SerialNumber":
                    expectedlog = $"The {updatedField} in table token has been changed from {Token.SerialNumber} to {newValue} by {admin.Account.UserID}";
                    break;
                case "Type":
                    expectedlog = $"The {updatedField} in table token has been changed from {Token.Type} to {newValue} by {admin.Account.UserID}";
                    break;
            }
            overviewPage.Search(Token.AssetTag);
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log is not as expected");
        }

        [Given(@"There is an active token existing")]
        public async Task GivenThereIsAnActiveOkenExisting()
        {
            Token = await context.CreateToken(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.TokenOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(Token.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I want to deactivete the token whith the reason (.*)")]
        public void WhenIWantToDeactiveteTheTokenWhithTheReasonTest(string reason)
        {
            newValue = reason;
            var deactivatePage = overviewPage.Deactivate();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deativate");
            deactivatePage.Reason = newValue;
            deactivatePage.Delete();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        [Then(@"the token is deactivated")]
        public void ThenTheTokenIsDeactivated()
        {
            expectedlog = $"The Token with type {Token.Type} in table token is deleted due to {newValue} by {admin.Account.UserID}";
            overviewPage.Search(Token.AssetTag);
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log is not as expected");
        }

        [Given(@"There is an inactive token existing")]
        public async Task GivenThereIsAnInactiveTokenExisting()
        {
            Token = await context.CreateToken(admin, false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.TokenOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(Token.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
        }
        [When(@"I want to activate this token")]
        public void WhenIWantToActivateThisToken()
        {
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Then(@"The token is active")]
        public void ThenTheTokenIsActive()
        {
            expectedlog = $"The Token with type {Token.Type} in table token is activated by {admin.Account.UserID}";
            overviewPage.Search(Token.AssetTag);
            var detail = overviewPage.Detail();
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log is not as expected");
        }

        [When(@"I assign the Token to the Identity")]
        public void WhenIAssignTheTokenToTheIdentity()
        {
            Identity = (entity.Identity)TestData.Get("Identity");
            AssignPage = overviewPage.AssignIdentity();
            AssignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignPage");
            AssignPage.Title.Should().BeEquivalentTo("Assign identity to Token", "Title should be correct");
            AssignPage.SelectIdentity(Identity);
            AssignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_IdentitySelected");
        }
        [When(@"I fill in the assign form for my Token")]
        public void WhenIFillInTheAssignFormForMyToken()
        {
            var assignForm = AssignPage.Assign();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignForm");
            assignForm.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            assignForm.Employee.Should().BeEquivalentTo(Identity.Name, "The employee should be the name of the identity");
            assignForm.CreatePDF();
            assignForm.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The Identity is assigned to the Token")]
        public void ThenTheIdentityIsAssignedToTheToken()
        {
            overviewPage.Search(Token.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Details");
            expectedlog = $"The Token with {Token.AssetTag} is assigned to Identity with name: {Identity.Name} by {admin.Account.UserID} in table token";
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }

        [Given(@"that Identity is assigned to my token")]
        public async Task GivenThatIdentityIsAssignedToMyToken()
        {
            Identity = (entity.Identity)TestData.Get("Identity");
            await context.AssignIdentity2Device(admin, Token, Identity);
        }
        [When(@"I release that identity from my token")]
        public void WhenIReleaseThatIdentityFromMyToken()
        {
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
            releaseIdenity = detail.ReleaseIdentity();
            releaseIdenity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeatilPage");
        }
        [When(@"I fill in the release form for my token")]
        public void WhenIFillInTheReleaseFormForMyToken()
        {
            releaseIdenity.Title.Should().BeEquivalentTo("Release identity from Token", "Title should be correct");
            releaseIdenity.ITEmployee.Should().BeEquivalentTo(admin.Account.UserID, "The IT employee should be the admin");
            releaseIdenity.Employee.Should().BeEquivalentTo(Identity.Name, "The employee should be the name of the identity");
            releaseIdenity.CreatePDF();
            releaseIdenity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The identity is released from my token")]
        public void ThenTheIdentityIsReleasedFromMyToken()
        {
            overviewPage.Search(Token.AssetTag);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Details");
            expectedlog = $"The Token with {Token.AssetTag} is released from Identity with name: {Identity.Name} by {admin.Account.UserID} in table token";
            string log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog, "Log should match");
        }

    }
}
