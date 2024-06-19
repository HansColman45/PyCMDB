using Bright.ScreenPlay.Abilities;
using Bright.ScreenPlay.Actors;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Tasks;
using CMDB.UI.Specflow.Tasks.Account;
using CMDB.UI.Specflow.Tasks.Identity;
using OpenQA.Selenium;

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
        /// <summary>
        /// The WebDriver
        /// </summary>
        protected IWebDriver Driver { get; set; }
        /// <summary>
        /// The expected log
        /// </summary>
        public  string ExpectedLog { get; set; }
        public CMDBActor(ScenarioContext scenarioContext, string name = "CMDB") : base(name)
        {
            IsAbleToDoOrUse<DataContext>();
            IsAbleToDoOrUse<LoginPage>();
            _scenarioContext = scenarioContext;
        }
        public async Task<Admin> CreateNewAdmin()
        {
            return await Perform(new CreateTheAdmin());
        }
        public bool IsTheUserLoggedIn => Perform(new IsTheUserLoggedIn());
        
        public void DoLogin(string userName, string password)
        {
            Driver = Perform(new OpenTheLoginPage());
            var page = GetAbility<LoginPage>();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Login");
            page.UserId = userName;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_EnterUserId");
            page.Password = password;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_EnterPassword");
            Perform(new OpenTheMainPage());
            IsAbleToDoOrUse<MainPage>();
            var mainPage = GetAbility<MainPage>();
            mainPage.WebDriver = Driver;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_LogedIn");
        }
        public string IdentityLastLogLine
        {
            get
            {
                Perform(new OpenTheIdentityDetailPage());
                IsAbleToDoOrUse<IdentityDetailPage>();
                var detail = GetAbility<IdentityDetailPage>();
                detail.WebDriver = Driver;
                detail.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
                return Perform(new TheIdentityDertailLastLogLine());
            }
        }
        public string AccountLastLogLine
        {
            get
            {
                Perform(new OpenTheAccountDetailPage());
                IsAbleToDoOrUse<AccountDetailPage>();
                var detail = GetAbility<AccountDetailPage>();
                detail.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
                return Perform(new TheAccountDertailLastLogLine());
            }
        }
        public IdentityOverviewPage OpenIdentityOverviewPage()
        {
            Perform(new OpenTheIdentityOverviewPage());
            IsAbleToDoOrUse<IdentityOverviewPage>();
            var overviewPage = GetAbility<IdentityOverviewPage>();
            overviewPage.WebDriver = Driver;
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_OverviewPage");
            return overviewPage;
        }
        public CreateIdentityPage OpenCreateIdentityPage()
        {
            Perform(new OpenTheCreateIdentityPage());
            IsAbleToDoOrUse<CreateIdentityPage>();
            var createPage = GetAbility<CreateIdentityPage>();
            createPage.WebDriver = Driver;
            createPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_CreatePage");
            return createPage;
        }
        public AccountOverviewPage OpenAccountOverviewPage()
        {
            Perform(new OpenTheAccountOverviewPage());
            IsAbleToDoOrUse<AccountOverviewPage>();
            var overviewPage = GetAbility<AccountOverviewPage>();
            overviewPage.WebDriver = Driver;
            overviewPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        public CreateAccountPage OpenAccountCreatePage()
        {
            Perform(new OpenTheAccountCreatePage());
            IsAbleToDoOrUse<CreateAccountPage>();
            var createPage = GetAbility<CreateAccountPage>();
            createPage.WebDriver = Driver;
            createPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_CreatePage");
            return createPage;
        }
        public void Dispose()
        {
            //var main = GetAbility<LoginPage>();
            //main.Dispose();
            //var db = GetAbility<DataContext>();
            //db.Dispose();
            var abilities = GetAbilities();
            foreach (var ability in abilities)
            {
                ability.Dispose();
            }
        }
    }
}
