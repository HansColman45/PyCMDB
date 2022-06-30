using CMDB.Testing.Helpers;
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
        public LoginSteps(ScenarioData scenarioData, ScenarioContext context) : base(scenarioData, context)
        {
        }

        [Given(@"I open the home page")]
        public void GivenIOpenTheHomePage()
        {
            ScenarioData.Driver.Navigate().GoToUrl(Settings.Url);
            login = new LoginPage(ScenarioData.Driver);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Login");
        }

        [When(@"I logon with a valid user and password")]
        public void WhenILogonWithAValidUserAndPassword()
        {
            login.EnterUserID(admin.Account.UserID);
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterUserId");
            login.EnterPassword("1234");
            login.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EneterPWD");
        }

        [Then(@"I can logon")]
        public void ThenICanLogon()
        {
            main = login.LogIn();
            main.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LogedIn");
            Assert.True(main.LoggedIn(), "user is not logged in");
        }

    }
}
