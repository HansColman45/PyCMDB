using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;
using CMDB.Testing.Helpers;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public sealed class AccountSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private AccountOverviewPage overviewPage;
        private CreateAccountPage createAccount;
        private AssignFormPage AssignFom;

        private readonly Random rnd = new();
        private int rndNr;
        private helpers.Account account;
        private entity.Account Account;
        private entity.Identity Identity;

        private string expectedlog, oldValue, newValue, changedField;

        public AccountSteps(ScenarioData scenarioData, ScenarioContext context) : base(scenarioData, context)
        {
        }
        #region Create
        [Given(@"I want to create a Account with the following details")]
        public void GivenIWantToCreateAAccountWithTheFollowingDetails(Table table)
        {
            account = table.CreateInstance<helpers.Account>();
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            if (Settings.TakeScreenShot)
                main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.AccountOverview();
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AccountOverview");
            createAccount = overviewPage.New();
            if (Settings.TakeScreenShot)
                createAccount.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ClickNew");
            createAccount.UserId = account.UserId + rndNr.ToString();
            if (Settings.TakeScreenShot)
                createAccount.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterUserId");
            createAccount.Type = account.Type;
            if (Settings.TakeScreenShot)
                createAccount.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectType");
            createAccount.Application = account.Application;
            if (Settings.TakeScreenShot)
                createAccount.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectAppliction");
        }
        [When(@"I save the account")]
        public void WhenISaveTheAccount()
        {
            createAccount.Create();
            if (Settings.TakeScreenShot)
                createAccount.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Saved");
        }
        [Then(@"The account is saved")]
        public void ThenTheAccountIsSaved()
        {
            overviewPage.Search(account.UserId + rndNr.ToString());
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            if (Settings.TakeScreenShot)
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            var log = detail.GetLastLog();
            if (Settings.TakeScreenShot)
                expectedlog = $"The Account width UserID: {account.UserId + rndNr.ToString()} with type {account.Type} for application {account.Application} is created by {admin.Account.UserID} in table account";
            Assert.Equal(expectedlog, log);
        }
        #endregion
        [Given(@"There is an account existing")]
        public async Task GivenThereIsAnAccountExisting()
        {
            Account = await context.CreateAccount(admin);
            rndNr = rnd.Next();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            if (Settings.TakeScreenShot)
                main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.AccountOverview();
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AccountOverview");
            overviewPage.Search(Account.UserID);
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
        #region update
        [When(@"I change the (.*) to (.*) and I save the changes")]
        public void WhenIChangeTheUserIdToTestjeAndISaveTheChanges(string field, string newValue)
        {
            var editPage = overviewPage.Edit();
            if (Settings.TakeScreenShot)
                editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditAccount");
            changedField = field;
            switch (field)
            {
                case "UserId":
                    oldValue = editPage.UserId;
                    this.newValue = newValue + rndNr.ToString();
                    editPage.UserId = this.newValue;
                    if (Settings.TakeScreenShot)
                        editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ChangedUserId");
                    break;
                case "Type":
                    oldValue = "Normal User";
                    this.newValue = newValue;
                    editPage.Type = newValue;
                    if (Settings.TakeScreenShot)
                        editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ChangedType");
                    break;
                case "Application":
                    oldValue = "CMDB";
                    this.newValue = newValue;
                    editPage.Application = newValue;
                    if (Settings.TakeScreenShot)
                        editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ChangedApplication");
                    break;
            }
            editPage.Edit();
            if (Settings.TakeScreenShot)
                editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Changed");
        }
        [Then(@"The changes in account are saved")]
        public void ThenTheChangesInAccountAreSaved()
        {
            switch (changedField)
            {
                case "UserId":
                    overviewPage.Search(newValue);
                    if (Settings.TakeScreenShot)
                        expectedlog = $"The UserId in table account has been changed from {oldValue} to {newValue} by {admin.Account.UserID}";
                    break;
                case "Type":
                    overviewPage.Search(Account.UserID);
                    if (Settings.TakeScreenShot)
                        expectedlog = $"The Type in table account has been changed from {oldValue} to {newValue} by {admin.Account.UserID}";
                    break;
                case "Application":
                    overviewPage.Search(Account.UserID);
                    if (Settings.TakeScreenShot)
                        expectedlog = $"The Application in table account has been changed from {oldValue} to {newValue} by {admin.Account.UserID}";
                    break;
            }
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            if (Settings.TakeScreenShot)
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }
        #endregion
        #region deactivate
        [Given(@"There is an active account existing")]
        public async Task GivenThereIsAnActiveAccountExisting()
        {
            Account = await context.CreateAccount(admin);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID); 
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            if (Settings.TakeScreenShot)
                main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.AccountOverview();
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AccountOverview");
            overviewPage.Search(Account.UserID);
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
        [When(@"I deactivate the account with reason (.*)")]
        public void WhenIDeactivateTheAccountWithReasonTest(string reason)
        {
            var deactivatepage = overviewPage.Deactivate();
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivate");
            deactivatepage.Reason = reason;
            newValue = reason;
            if (Settings.TakeScreenShot)
                deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterReason");
            deactivatepage.Delete();
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        [Then(@"the account is inactive")]
        public void ThenTheAccountIsInactive()
        {
            overviewPage.Search(Account.UserID);
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            if (Settings.TakeScreenShot)
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            var accId = detail.Id;
            entity.Account account = context.GetAccount(accId);
            expectedlog = $"The Account width UserID: {account.UserID} and type {account.Type.Description} in table account is deleted due to {newValue} by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }
        #endregion
        #region Activate
        [Given(@"There is an inactive account existing")]
        public async Task GivenThereIsAnInactiveAccountExisting()
        {
            Account = await context.CreateAccount(admin, false);
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            if (Settings.TakeScreenShot)
                login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            if (Settings.TakeScreenShot)
                main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.AccountOverview();
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AccountOverview");
            overviewPage.Search(Account.UserID);
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
        [When(@"I activate the account")]
        public void WhenIActivateTheAccount()
        {
            overviewPage.Activate();
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Then(@"The account is active")]
        public void ThenTheAccountIsActive()
        {
            overviewPage.Search(Account.UserID);
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            if (Settings.TakeScreenShot)
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            var accId = detail.Id;
            entity.Account account = context.GetAccount(accId);
            if (Settings.TakeScreenShot)
                expectedlog = $"The Account width UserID: {account.UserID} and type {account.Type.Description} in table account is activated by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }
        #endregion
        #region Assign iden to acc
        [When(@"I assign the identity to my account")]
        public void WhenIAssignTheIdentityToMyAccount()
        {
            Identity = (entity.Identity)TestData.Get("Identity");
            var assign = overviewPage.AssignIdentity();
            if (Settings.TakeScreenShot)
                assign.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignIdentity");
            assign.SelectIdentity(Identity);
            if (Settings.TakeScreenShot)
                assign.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectIdentity");
            DateTime validFrom = DateTime.Now.AddYears(-1);
            DateTime validUntil = DateTime.Now.AddYears(+1);
            assign.ValidFrom = validFrom.ToString("dd/MM/yyyy\tHH:mm\tt");
            if (Settings.TakeScreenShot)
                assign.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterValidFrom");
            assign.ValidUntil = validUntil.ToString("dd/MM/yyyy\tHH:mm\tt");
            if (Settings.TakeScreenShot)
                assign.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterValidEnt");
            AssignFom = assign.Assign();
            if (Settings.TakeScreenShot)
                AssignFom.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignForm");
            Assert.False(AssignFom.IsVaidationErrorVisable());
        }
        [When(@"I fill in the assig form for my account")]
        public void WhenIFillInTheAssigForm()
        {
            string naam = AssignFom.Name;
            Assert.Equal(naam, AssignFom.Employee);
            AssignFom.CreatePDF();
            if (Settings.TakeScreenShot)
                AssignFom.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_PDFCreated");
        }
        [Then(@"The identity is assigned to my account")]
        public void ThenTheIdentityIsAssignedToMyAccount()
        {
            overviewPage.Search(Account.UserID);
            if (Settings.TakeScreenShot)
                overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            if (Settings.TakeScreenShot)
                detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            if (Settings.TakeScreenShot)
                expectedlog = $"The Account with UserID {Account.UserID} in table account is assigned to Identity width name: {Identity.Name} by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            Assert.Equal(expectedlog, log);
        }
        #endregion
    }
}
