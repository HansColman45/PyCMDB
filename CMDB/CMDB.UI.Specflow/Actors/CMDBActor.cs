using Bright.ScreenPlay.Actors;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Questions.Identity;
using CMDB.UI.Specflow.Questions.Account;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors
{
    public class CMDBActor : Actor
    {
        /// <summary>
        /// The ScenatioContext
        /// </summary>
        protected readonly ScenarioContext _scenarioContext;
        /// <summary>
        /// Random number generator
        /// </summary>
        protected readonly Random rnd = new();
        /// <summary>
        /// The Nlog logger
        /// </summary>
        protected readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The Admin that will login
        /// </summary>
        protected Admin admin;
        /// <summary>
        /// The random number
        /// </summary>
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
            return await Perform(new TheAdmin());
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
        public string IdentityLastLogLine
        {
            get
            {
                var detail = Perform(new OpenTheIdentityDetailPage());
                IsAbleToDoOrUse(detail);
                detail.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
                return Perform(new TheIdentityDertailLastLogLine());
            }
        }
        public string AccountLastLogLine
        {
            get
            {
                var detail = Perform(new OpenTheAccountDetailPage());
                IsAbleToDoOrUse(detail);
                detail.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
                return Perform(new TheAccountDertailLastLogLine());
            }
        }
        public void Dispose()
        {
            var main = GetAbility<LoginPage>();
            main.Dispose();
            var db = GetAbility<DataContext>();
            db.Dispose();
        }
    }
}
