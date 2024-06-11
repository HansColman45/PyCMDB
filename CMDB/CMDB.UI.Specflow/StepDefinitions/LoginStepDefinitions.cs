using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Tasks;
using Microsoft.VisualBasic;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public class LoginStepDefinitions: TestBase
    {
        private Actor actor;
        private LoginPage loginPage;
        private MainPage mainPage;

        public LoginStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        [Given(@"I open the home page")]
        public async Task GivenIOpenTheHomePage()
        {
            actor = new("Joe");
            actor.IsAbleToDoOrUse<LoginPage>();
            actor.IsAbleToDoOrUse<DataContext>();
            Admin = await TheAdmin.CreateNewAdminAs(actor);
            loginPage = OpenTheLoginPageTask.OpenLoginPageAs(actor); 
        }

        [When(@"I logon with a valid user name and password")]
        public void WhenILogonWithAValidUserNameAndPassword()
        {
            mainPage = LoginTask.LoginAs(actor,Admin.Account.UserID,"1234");
            //actor.IsAbleToDoOrUse(mainPage);
        }

        [Then(@"I can logon")]
        public void ThenICanLogon()
        {
            TheUserIsLoggedIn.IsLoggedInAs(actor).Should().BeTrue();
        }
    }
}
