using Bright.ScreenPlay.Actors;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Abilities.Pages.Admin;
using CMDB.UI.Specflow.Abilities.Pages.Application;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;
using CMDB.UI.Specflow.Abilities.Pages.Permissions;
using CMDB.UI.Specflow.Abilities.Pages.Role;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;
using CMDB.UI.Specflow.Abilities.Pages.System;
using CMDB.UI.Specflow.Abilities.Pages.Token;
using CMDB.UI.Specflow.Abilities.Pages.Types;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Questions.Account;
using CMDB.UI.Specflow.Questions.Admin;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Main;
using OpenQA.Selenium;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors
{
    /// <summary>
    /// The main actor for the CMDB
    /// </summary>
    public class CMDBActor : Actor
    {
        private MainPage mainPage;
        /// <summary>
        /// The ScenatioContext
        /// </summary>
        protected readonly ScenarioContext ScenarioContext;
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
        private IWebDriver Driver { get; set; }
        /// <summary>
        /// The expected log
        /// </summary>
        public  string ExpectedLog { get; set; }
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="scenarioContext"></param>
        /// <param name="name">The name of the actor</param>
        public CMDBActor(ScenarioContext scenarioContext, string name = "CMDB") : base(name)
        {
            IsAbleToDoOrUse<DataContext>();
            ScenarioContext = scenarioContext;
        }
        /// <summary>
        /// This will create a new admin
        /// </summary>
        /// <returns><see cref="Admin"/></returns>
        public async Task<Admin> CreateNewAdmin()
        {
            try
            {
                admin = await Perform(new CreateTheAdmin());
                log.Info($"We will use Admin with UserId: {admin.Account.UserID} to logon");
                return admin;
            }
            catch (Exception e)
            {
                log.Fatal(e.Message);
                throw;
            }
        }
        /// <summary>
        /// This will login to the CMDB
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void DoLogin(string userName, string password)
        {
            try
            {
                var page = Perform(new OpenTheLoginPage());
                //Driver = page.WebDriver;
                page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Login");
                page.UserId = userName;
                page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterUserId");
                page.Password = password;
                page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EnterPassword");
                mainPage = Perform(new OpenTheMainPage());
                page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LogedIn");
            }
            catch (Exception e)
            {
                log.Fatal(e.Message);
                throw;
            }
        }
        /// <summary>
        /// This will open the Identity overview page
        /// </summary>
        /// <returns><see cref="IdentityOverviewPage"/></returns>
        public IdentityOverviewPage OpenIdentityOverviewPage()
        {
            var overviewPage = Perform(new OpenTheIdentityOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_OverviewPage");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Account overview page
        /// </summary>
        /// <returns><see cref="AccountOverviewPage"/></returns>
        public AccountOverviewPage OpenAccountOverviewPage()
        {
            var overviewPage = Perform(new OpenTheAccountOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Role overview page
        /// </summary>
        /// <returns><see cref="RoleOverviewPage"/></returns>
        public RoleOverviewPage OpenRoleOverviewPage()
        {
            var overviewPage = Perform(new OpenTheRoleOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Device overview page
        /// </summary>
        /// <returns><see cref="LaptopOverviewPage"/></returns>
        public LaptopOverviewPage OpenLaptopOverviewPage()
        {
            var overviewPage = Perform(new OpenTheLaptopOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Desktop overview page
        /// </summary>
        /// <returns><see cref="DesktopOverviewPage"/></returns>
        public DesktopOverviewPage OpenDesktopOverviewPage()
        {
            var overviewPage = Perform(new OpenTheDesktopOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Monitor overview page
        /// </summary>
        /// <returns><see cref="MonitorOverviewPage"/></returns>
        public MonitorOverviewPage OpenMonitorOverviewPage()
        {
            var overviewPage = Perform(new OpenTheMonitorOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Docking overview page
        /// </summary>
        /// <returns><see cref="DockingOverviewPage"/></returns>
        public DockingOverviewPage OpenDockingOverviewPage()
        {
            var overviewPage = Perform(new OpenTheDockingOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Token overview page
        /// </summary>
        /// <returns><see cref="TokenOverviewPage"/></returns>
        public TokenOverviewPage OpenTokenOverviewPage()
        {
            var overviewPage = Perform(new OpenTheTokenOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Kensington overview page
        /// </summary>
        /// <returns><see cref="KensingtonOverviewPage"/></returns>
        public KensingtonOverviewPage OpenKensingtonOverviewPage()
        {
            var overviewPage = Perform(new OpenTheKensingtonOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Mobile overview page
        /// </summary>
        /// <returns><see cref="MobileOverviewPage"/></returns>
        public MobileOverviewPage OpenMobileOverviewPage()
        {
            var overviewPage = Perform(new OpenTheMobileOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Subscription overview page
        /// </summary>
        /// <returns><see cref="SubscriptionOverviewPage"/></returns>
        public SubscriptionOverviewPage OpenSubscriptionOverviewPage()
        {
            var overviewPage = Perform(new OpenTheSubscriptionOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the AssetType overview page
        /// </summary>
        /// <returns><see cref="AssetTypeOverviewPage"/></returns>
        public AssetTypeOverviewPage OpenAssetTypeOverviewPage()
        {
            var overviewPage = Perform(new OpenAssetTypeOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the AssetCategory overview page
        /// </summary>
        /// <returns><see cref="AssetCategoryOverviewPage"/></returns>
        public AssetCategoryOverviewPage OpenAssetCategoryOverviewPage()
        {
            var overviewPage = Perform(new OpenAssetCategoryOverview());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Identity Type overview page
        /// </summary>
        /// <returns><see cref="TypeOverviewPage"/></returns>
        public TypeOverviewPage OpenIdentityTypeOverviewPage()
        {
            var overviewPage = Perform(new OpenTheIdentityTypeOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Account Type overview page
        /// </summary>
        /// <returns><see cref="TypeOverviewPage"/></returns>
        public TypeOverviewPage OpenAccountTypeOverviewPage()
        {
            var overviewPage = Perform(new OpenTheAccountTypeOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Role Type overview page
        /// </summary>
        /// <returns><see cref="TypeOverviewPage"/></returns>
        public TypeOverviewPage OpenRoleTypeOverviewPage()
        {
            var overviewPage = Perform(new OpenTheOpenRoleTypeOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Subscription Type overview page
        /// </summary>
        /// <returns><see cref="SubscriptionTypeOverviewPage"/></returns>
        public SubscriptionTypeOverviewPage OpenSubscriptionTypeOverviewPage()
        {
            var overviewPage = Perform(new OpenTheSubscriptionTypeOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the System overview page
        /// </summary>
        /// <returns><see cref="SystemOverviewPage"/></returns>
        public SystemOverviewPage OpenSystemOverviewPage()
        {
            var overviewPage = Perform(new OpenTheSystemOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Application overview page
        /// </summary>
        /// <returns><see cref="ApplicationOverviewPage"/></returns>
        public ApplicationOverviewPage OpenApplicationOverviewPage()
        {
            var overviewPage = Perform(new OpenTheApplicationOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Admin overview page
        /// </summary>
        /// <returns><see cref="AdminOverviewPage"/></returns>
        public AdminOverviewPage OpenAdminOverviewPage()
        {
            var overviewPage = Perform(new OpenTheAdminOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will open the Permission overview page
        /// </summary>
        /// <returns><see cref="PermissionOverviewPage"/></returns>
        public PermissionOverviewPage OpenPermissionOverviewPage()
        {
            var overviewPage = Perform(new OpenThePermissionOverviewPage());
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            return overviewPage;
        }
        /// <summary>
        /// This will click the search textbox and enter the search string
        /// </summary>
        /// <param name="search"></param>
        public void Search(string search)
        {
            var page = GetAbility<MainPage>();
            page.Search(search);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
        /// <summary>
        /// This will open the Assign form page
        /// </summary>
        /// <returns><see cref="AssignFormPage"/></returns>
        public AssignFormPage OpenAssignFom()
        {
            var page = Perform(new OpenTheAssignFormPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssignForm");
            return page;
        }
        /// <summary>
        /// This will dispose the actor
        /// </summary>
        public void Dispose()
        {
            var main = GetAbility<LoginPage>();
            if(main.WebDriver != null)
                main.Dispose();
            var db = GetAbility<DataContext>();
            if(db.context != null)
                db.Dispose();
        }
        /// <summary>
        /// This will return the RAM object based on the display name
        /// </summary>
        /// <param name="notConvertedRam"></param>
        /// <returns></returns>
        protected RAM GetRam(string notConvertedRam)
        {
            var AllRams = Perform(new GetAllRAMInfo());
            return AllRams.FirstOrDefault(x => x.Display == notConvertedRam);
        }
        /// <summary>
        /// This will create or get the AssetCategory
        /// </summary>
        /// <param name="assetCategory">The assetcategory</param>
        /// <param name="vendor">The vendor</param>
        /// <param name="type">The type</param>
        /// <returns><see cref="AssetType"/></returns>
        protected async Task<AssetType> GetOrCreateAssetType(string assetCategory, string vendor, string type)
        {
            var context = GetAbility<DataContext>();
            var assetCat = context.GetAssetCategory(assetCategory);
            return await context.GetOrCreateAssetType(vendor, type, assetCat, admin);
        }
        
    }
}
