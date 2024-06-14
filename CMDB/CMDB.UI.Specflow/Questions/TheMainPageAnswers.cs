using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
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

namespace CMDB.UI.Specflow.Questions
{
    public class OpenTheIdentityOverviewPage : Question<IdentityOverviewPage>
    {
        public override IdentityOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Identity']");
            page.ClickElementByXpath("//a[@id='Identity']");
            page.ClickElementByXpath("//a[@id='Identity2']");
            page.ClickElementByXpath("//a[@href='/Identity']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheAccountOverviewPage : Question<AccountOverviewPage>
    {
        public override AccountOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Account']");
            page.ClickElementByXpath("//a[@id='Account']");
            page.ClickElementByXpath("//a[@id='Account2']");
            page.ClickElementByXpath("//a[@href='/Account']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheLaptopOverviewPage : Question<LaptopOverviewPage>
    {
        public override LaptopOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Laptop11']");
            page.ClickElementByXpath("//a[@href='/Laptop']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheDesktopOverviewPage : Question<DesktopOverviewPage>
    {
        public override DesktopOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Desktop13']");
            page.ClickElementByXpath("//a[@href='/Desktop']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheMonitorOverviewPage : Question<MonitorOverviewPage>
    {
        public override MonitorOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Monitor15']");
            page.ClickElementByXpath("//a[@href='/Monitor']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheDockingOverviewPage : Question<DockingOverviewPage>
    {
        public override DockingOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Docking station17']");
            page.ClickElementByXpath("//a[@href='/Docking']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheTokenOverviewPage : Question<TokenOverviewPage>
    {
        public override TokenOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Token19']");
            page.ClickElementByXpath("//a[@href='/Token']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheMobileOverviewPage : Question<MobileOverviewPage>
    {
        public override MobileOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Mobile23']");
            page.ClickElementByXpath("//a[@href='/Mobile']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheSubscriptionOverviewPage : Question<SubscriptionOverviewPage>
    {
        public override SubscriptionOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Subscription25']");
            page.ClickElementByXpath("//a[@href='/Subscription']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheAssetTypeOverviewPage : Question<AssetTypeOverviewPage>
    {
        public override AssetTypeOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Asset Type28']");
            page.ClickElementByXpath("//a[@href='/AssetType']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        }
    }
    public class TheIdentityTypeOverviewPage : Question<TypeOverviewPage> 
    { 
        public override TypeOverviewPage PerformAs(IPerformer actor) 
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Identity Type32']");
            page.ClickElementByXpath("//a[@href='/IdentityType']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        } 
    }
    public class TheAccountTypeOverviewPage : Question<TypeOverviewPage> 
    { 
        public override TypeOverviewPage PerformAs(IPerformer actor) 
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Account Type34']");
            page.ClickElementByXpath("//a[@href='/AccountType']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        } 
    }
    public class TheOpenRoleTypeOverviewPage : Question<TypeOverviewPage> 
    { 
        public override TypeOverviewPage PerformAs(IPerformer actor) 
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Role Type36']");
            page.ClickElementByXpath("//a[@href='/RoleType']");
            page.WaitOnAddNew();
            return new(page.WebDriver);
        } 
    }
    public class TheSubscriptionTypeOverviewPage : Question<SubscriptionTypeOverviewPage>
    {
        public override SubscriptionTypeOverviewPage PerformAs(IPerformer actor)
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
    }
}
