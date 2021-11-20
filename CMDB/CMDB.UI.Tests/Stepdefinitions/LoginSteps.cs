using CMDB.UI.Tests.Hooks;
using CMDB.UI.Tests.Pages;
using TechTalk.SpecFlow;
using Xunit;

namespace CMDB.UI.Tests.Stepdefinitions
{
    [Binding]
    public sealed class LoginSteps : TestBase
    {
        private LoginPage login;
        private MainPage main;
        public LoginSteps(ScenarioData scenarioData) : base(scenarioData)
        {
        }

        [Given(@"I open the home page")]
        public void GivenIOpenTheHomePage()
        {
            Url = "https://localhost:44314/";
            ScenarioData.Driver.Navigate().GoToUrl(Url);
            login = new LoginPage(ScenarioData.Driver);
        }

        [When(@"I logon with a valid user and password")]
        public void WhenILogonWithAValidUserAndPassword()
        {
            login.EnterUserID(admin.Account.UserID);
            login.EnterPassword("1234");
        }

        [Then(@"I can logon")]
        public void ThenICanLogon()
        {
            main = login.LogIn();
            Assert.True(main.LoggedIn(), "user is not logged in");
        }

    }
}
