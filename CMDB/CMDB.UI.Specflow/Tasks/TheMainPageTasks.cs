using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;
using CMDB.UI.Specflow.Abilities.Pages.Subscription;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;
using CMDB.UI.Specflow.Abilities.Pages.Token;
using CMDB.UI.Specflow.Abilities.Pages.Types;
using OpenQA.Selenium;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks
{
    public class TheMainPageTasks : Task
    {
        public override void PerformAs(IPerformer actor){ }
        public static IdentityOverviewPage OpenIdentityOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Identity']");
            page.ClickElementByXpath("//a[@id='Identity']");
            page.ClickElementByXpath("//a[@id='Identity2']");
            page.ClickElementByXpath("//a[@href='/Identity']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static AccountOverviewPage OpenAccountOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Account']");
            page.ClickElementByXpath("//a[@id='Account']");
            page.ClickElementByXpath("//a[@id='Account5']");
            page.ClickElementByXpath("//a[@href='/Account']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static void OpenRoleOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Role']");
            page.ClickElementByXpath("//a[@id='Role']");
            page.ClickElementByXpath("//a[@id='Role8']");
            page.ClickElementByXpath("//a[@href='/Role']");
            page.WaitOnAddNew();
            //return new(page.WebDriver);
        }
        public static LaptopOverviewPage OpenLaptopOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Laptop11']");
            page.ClickElementByXpath("//a[@href='/Laptop']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static DesktopOverviewPage OpenDesktopOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Desktop13']");
            page.ClickElementByXpath("//a[@href='/Desktop']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static MonitorOverviewPage OpenMonitorPageAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Monitor15']");
            page.ClickElementByXpath("//a[@href='/Monitor']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static DockingOverviewPage OpenDockingOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Docking station17']");
            page.ClickElementByXpath("//a[@href='/Docking']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static TokenOverviewPage OpenTokenOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Token19']");
            page.ClickElementByXpath("//a[@href='/Token']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static void OpenKensingtonOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Kensington21']");
            page.ClickElementByXpath("//a[@href='/Kensington']");
            page.WaitOnAddNew();
            //return new(page.WebDriver);
        }
        public static MobileOverviewPage OpenMobileOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Mobile23']");
            page.ClickElementByXpath("//a[@href='/Mobile']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }       
        public static SubscriptionOverviewPage OpenSubscriptionOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Subscription25']");
            page.ClickElementByXpath("//a[@href='/Subscription']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static AssetTypeOverviewPage OpenAssetTypeOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Asset Type28']");
            page.ClickElementByXpath("//a[@href='/AssetType']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static void OpenAssetCategoryOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Asset Category30']");
            page.ClickElementByXpath("//a[@href='/AssetCategory']");
            page.WaitOnAddNew();
            //return new(page.WebDriver);
        }
        public static TypeOverviewPage OpenIdentityTypeOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Identity Type32']");
            page.ClickElementByXpath("//a[@href='/IdentityType']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static TypeOverviewPage OpenAccountTypeOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Account Type34']");
            page.ClickElementByXpath("//a[@href='/AccountType']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static TypeOverviewPage OpenRoleTypeOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Role Type36']");
            page.ClickElementByXpath("//a[@href='/RoleType']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static SubscriptionTypeOverviewPage OpenSubscriptionTypeOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.WaitUntilElmentVisableByXpath("//a[@id='Subscription Type38']");
            page.ClickElementByXpath("//a[@id='Subscription Type38']");
            page.ClickElementByXpath("//a[@href='/SubscriptionType']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
        public static void OpenSystemOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='System']");
            page.ClickElementByXpath("//a[@id='System']");
            page.ClickElementByXpath("//a[@id='System41']");
            page.ClickElementByXpath("//a[@href='/System']");
            page.WaitOnAddNew();
            //return new(page.WebDriver);
        }
        public static void ApplicationOverview(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Application']");
            page.ClickElementByXpath("//a[@id='Application']");
            page.ClickElementByXpath("//a[@id='Application44']");
            page.ClickElementByXpath("//a[@href='/Application']");
            page.WaitOnAddNew();
            //return new(page.WebDriver);
        }
        public static void OpenAdminOverviewAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin47']");
            page.ClickElementByXpath("//a[@href='/Admin']");
            page.WaitOnAddNew();
            //return new(page.WebDriver);
        }
        public static void PermissionOverview(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Permissions49']");
            page.ClickElementByXpath("//a[@href='/Permission']");
            page.WaitOnAddNew();
            //return new(page.WebDriver);
        }
    }
}
