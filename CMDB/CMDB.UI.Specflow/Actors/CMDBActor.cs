using Bright.ScreenPlay.Actors;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors
{
    public class CMDBActor : Actor
    {
        protected readonly ScenarioContext _scenarioContext;
        protected readonly Random rnd = new();
        protected Admin admin;
        protected int rndNr;
        public  string ExpectedLog { get; set; }
        public CMDBActor(ScenarioContext scenarioContext, string name = "CMDB") : base(name)
        {
            IsAbleToDoOrUse<DataContext>();
            IsAbleToDoOrUse<LoginPage>();
            _scenarioContext = scenarioContext;
        }
        public async Task<Admin> CreateNewAdmin()
        {
            var context = GetAbility<DataContext>();
            admin = await context.CreateNewAdmin();
            return admin;
        }
        public void DoLogin(string userName, string password)
        {
            Perform<OpenTheLoginPageTasks>();
            var page = GetAbility<LoginPage>();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Login");
            page.UserId = userName;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_EnterUserId");
            page.Password = password;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_EnterPassword");
            MainPage mainPage = Perform(new OpenTheMainPage());
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_LogedIn");
            IsAbleToDoOrUse(mainPage);
        }
        
    }
}
