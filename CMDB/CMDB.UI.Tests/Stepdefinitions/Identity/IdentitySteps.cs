using CMDB.UI.Tests.Helpers;
using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.UnitTestProvider;
using Xunit;
using Xunit.Extensions.Ordering;
using helpers = CMDB.UI.Tests.Helpers;
using entity = CMDB.Domain.Entities;
using System.Threading.Tasks;
using CMDB.Testing.Helpers;
using FluentAssertions;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding, Order(1)]
    public sealed class IdentitySteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        private IdentityOverviewPage overviewPage;
        private CreateIdentityPage createIdentity;
        private UpdateIdentityPage updateIdentity;
        private AssignAccountPage assignAccount;
        private AssignFormPage AssignFom;

        private readonly IUnitTestRuntimeProvider _unitTestRuntimeProvider;
        private readonly Random rnd = new();
        private int rndNr;
        private helpers.Identity iden;
        private entity.Account Account;
        private entity.Identity Identity;
        private entity.Device device;
        private string updatedfield, newvalue, reason;
        public IdentitySteps(ScenarioData scenarioData, IUnitTestRuntimeProvider unitTestRuntimeProvider, ScenarioContext context) : base(scenarioData, context)
        {
            _unitTestRuntimeProvider = unitTestRuntimeProvider;
        }
        #region create
        [Order(1)]
        [Given(@"I want to create an Identity with these details")]
        public void GivenIWantToCreateAnIdentityWithTheseDetails(Table table)
        {
            rndNr = rnd.Next();
            iden = table.CreateInstance<Identity>();
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.IdentityOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            createIdentity = overviewPage.New();
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            createIdentity.FirstName = iden.FirstName + rndNr.ToString();
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_FirstName");
            createIdentity.LastName = iden.LastName + rndNr.ToString();
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LastName");
            createIdentity.Email = iden.Email;
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Email");
            createIdentity.Company = iden.Company;
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Company");
            createIdentity.UserId = iden.UserId + rndNr.ToString();
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UserId");
            createIdentity.Type = iden.Type;
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            createIdentity.Language = iden.Language;
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
        }
        [Order(2)]
        [When(@"I save")]
        public void WhenISave()
        {
            createIdentity.Create();
            createIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Saved");
        }
        [Order(3)]
        [Then(@"I can find the newly created Identity back")]
        public void ThenICanFindTheNewlyCreatedIdenotyBack()
        {
            overviewPage.Search(iden.FirstName + rndNr.ToString());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            var log = detail.GetLastLog();
            string expectedlog = $"The Identity width name: {iden.FirstName + rndNr.ToString()}, {iden.LastName + rndNr.ToString()} is created by {admin.Account.UserID} in table identity";
            log.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        [Order(4)]
        [Given(@"An Identity exisist in the system")]
        public async Task GivenAnIdentityExisistInTheSystem()
        {
            Identity = await context.CreateIdentity(admin);
        }
        #region Update
        [Order(5)]
        [When(@"I want to update (.*) with (.*)")]
        public void WhenIWantToUpdateFirstNameWithTestje(string field, string value)
        {
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.IdentityOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            rndNr = rnd.Next();
            updateIdentity = overviewPage.Update();
            updateIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UpdatePage");
            iden = new();
            iden.FirstName = updateIdentity.FirstName.Trim();
            iden.LastName = updateIdentity.LastName.Trim();
            iden.Company = updateIdentity.Company.Trim();
            iden.UserId = updateIdentity.UserId.Trim();
            iden.Email = updateIdentity.Email.Trim();
            switch (field)
            {
                case "FirstName":
                    updatedfield = field;
                    newvalue = value + rndNr.ToString();
                    updateIdentity.FirstName = newvalue;
                    updateIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_FirstName");
                    break;
                case "LastName":
                    updatedfield = field;
                    newvalue = value + rndNr.ToString();
                    updateIdentity.LastName = newvalue;
                    updateIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LastName");
                    break;
                case "Company":
                    updatedfield = field;
                    newvalue = value + rndNr.ToString();
                    updateIdentity.Company = newvalue;
                    updateIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Company");
                    break;
                case "UserID":
                    updatedfield = field;
                    newvalue = value + rndNr.ToString();
                    updateIdentity.UserId = newvalue;
                    updateIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UserID");
                    break;
                case "Email":
                    updatedfield = field;
                    newvalue = value;
                    updateIdentity.Email = newvalue;
                    updateIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EMail");
                    break;
            }
            updateIdentity.Update();
            updateIdentity.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Updated");
        }
        [Order(6)]
        [Then(@"The identity is updated")]
        public void ThenTheIdentityIsUpdated()
        {
            string expectedlog = "";
            switch (updatedfield)
            {
                case "FirstName":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.FirstName} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(newvalue);
                    break;
                case "LastName":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.LastName} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(newvalue);
                    break;
                case "Company":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.Company} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(iden.FirstName);
                    break;
                case "UserID":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.UserId} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(iden.FirstName);
                    break;
                case "Email":
                    expectedlog = $"The {updatedfield} in table identity has been changed from {iden.Email} to {newvalue} by {admin.Account.UserID}";
                    overviewPage.Search(iden.FirstName);
                    break;
            }
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            var log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        #region Deactivate
        [Order(7)]
        [Given(@"An acive Identity exisist in the system")]
        public async Task GivenAnAciveIdentityExisistInTheSystem()
        {
            Identity = await context.CreateIdentity(admin);
        }
        [Order(8)]
        [When(@"I want to deactivete the identity whith the reason (.*)")]
        public void WhenIWantToDeactiveteTheIdentityWhithTheReasonTest(string reason)
        {
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            this.reason = reason;
            var deactivatepage = overviewPage.Deactivate();
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            deactivatepage.Reason = reason;
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_reason");
            deactivatepage.Delete();
            deactivatepage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deleted");
        }
        [Order(9)]
        [Then(@"The Idenetity is inactive")]
        public void ThenTheIdenetityIsInactive()
        {
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            int Id = detail.Id;
            entity.Identity iden = context.GetIdentity(Id);
            string expectedlog = $"The Identity width name: {iden.Name} in table identity is deleted due to {reason} by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        #region Activate
        [Order(10)]
        [Given(@"An inactive Identity exisist in the system")]
        public async Task GivenAnInactiveIdentityExisistInTheSystem()
        {
            Identity = await context.CreateIdentity(admin, false);
        }
        [Order(11)]
        [When(@"I want to activate this identity")]
        public void WhenIWantToActivateThisIdentity()
        {
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.IdentityOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
        [Order(12)]
        [Then(@"The Identity is active")]
        public void ThenTheIdentityIsActive()
        {
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Detail");
            string expectedlog = $"The Identity width name: {Identity.Name} in table identity is activated by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog);
        }
        #endregion
        #region Assign Account
        [Given(@"an Account exist as well")]
        public async Task GivenAnAccountExistAsWell()
        {
            Account = await context.CreateAccount(admin);
        }
        [When(@"I assign the account to the identity")]
        public void WhenIAssignTheAccountToTheIdentity()
        {
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.IdentityOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            assignAccount = overviewPage.AssignAccount();
            assignAccount.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignAccount");
            iden = new()
            {
                UserId = assignAccount.UserId,
                Email = assignAccount.EMail,
                Type = assignAccount.Type,
                FirstName = assignAccount.Name.Split(",")[1].Trim(),
                LastName = assignAccount.Name.Split(",")[0]
            };
            assignAccount.SelectAccount(Account);
            assignAccount.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectIdentity");
            DateTime validFrom = DateTime.Now.AddYears(-1);
            DateTime validUntil = DateTime.Now.AddYears(+1);
            assignAccount.ValidFrom = validFrom;
            assignAccount.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ValidFrom");
            assignAccount.ValidUntil = validUntil;
            assignAccount.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ValidUntil");
            AssignFom = assignAccount.Assign();
            AssignFom.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Assigned");
            Assert.False(assignAccount.IsVaidationErrorVisable());
        }
        [When(@"I fill in the assig form")]
        public void WhenIFillInTheAssigForm()
        {
            string naam = AssignFom.Name;
            Assert.Equal(naam, AssignFom.Employee);
            AssignFom.CreatePDF();
            AssignFom.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePDF");
        }
        [Then(@"The account is assigned to the idenity")]
        public void ThenTheAccountIsAssignedToTheIdenity()
        {
            overviewPage.Search(iden.UserId);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            int Id = detail.Id;
            entity.Identity identity = context.GetIdentity(Id);
            string expectedlog = $"The Identity with name: {identity.FirstName}, {identity.LastName} in table identity is assigned to Account with UserID: {Account.UserID} by {admin.Account.UserID}";
            var log = detail.GetLastLog();
            log.Should().BeEquivalentTo(expectedlog,"The log should match");
        }
        #endregion
        #region Assign Devices
        [Given(@"a (.*) exist as well")]
        public async void GivenALaptopExistAsWell(string category)
        {
            switch (category)
            {
                case "Laptop":
                    device = await context.CreateLaptop(admin);
                    break;
                case "Desktop":
                    device = await context.CreateDesktop(admin);
                    break;
                case "Token":
                    device = await context.CreateToken(admin);
                    break;
                case "Monitor":
                    device = await context.CreateMonitor(admin);
                    break;
                case "Docking":
                    device = await context.CreateDocking(admin);
                    break;
            }
        }
        [When(@"I assign that (.*) to the identity")]
        public void WhenIAssignThatLaptopToTheIdentity(string category)
        {
            log.Debug($"Assing device of type: {category}");
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Start");
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SelectUser");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPwd");
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Logedin");
            overviewPage = main.IdentityOverview();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            overviewPage.Search(Identity.FirstName);
            var assignPage = overviewPage.AssignDevice();
            assignPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignDevice");
            assignPage.ClickDevice(device);
            AssignFom = assignPage.Assign();
        }
        [Then(@"The (.*) is assigned")]
        public void ThenTheLaptopIsAssigned(string category)
        {
            log.Debug($"Check if device of type: {category} is assigned");
            overviewPage.Search(Identity.FirstName);
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Searched");
            var detail = overviewPage.Detail();
            detail.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
            string expectedlog = $"The Identity with name: {Identity.FirstName}, {Identity.LastName} is assigned to {device.Category.Category} " +
                        $"with {device.AssetTag} by {admin.Account.UserID} in table identity";
            var GetLastlog = detail.GetLastLog();
            GetLastlog.Should().BeEquivalentTo(expectedlog, "The log should match");
        }
        #endregion
    }
}
