using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks
{
    public class OpenAssetCategoryOverview : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Asset Category30']");
            page.ClickElementByXpath("//a[@href='/AssetCategory']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheIdentityOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Identity']");
            page.ClickElementByXpath("//a[@id='Identity']");
            page.ClickElementByXpath("//a[@id='Identity2']");
            page.ClickElementByXpath("//a[@href='/Identity']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheAccountOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Account']");
            page.ClickElementByXpath("//a[@id='Account']");
            page.ClickElementByXpath("//a[@id='Account2']");
            page.ClickElementByXpath("//a[@href='/Account']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheLaptopOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Laptop11']");
            page.ClickElementByXpath("//a[@href='/Laptop']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheDesktopOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Desktop13']");
            page.ClickElementByXpath("//a[@href='/Desktop']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheMonitorOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Monitor15']");
            page.ClickElementByXpath("//a[@href='/Monitor']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheDockingOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Docking station17']");
            page.ClickElementByXpath("//a[@href='/Docking']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheTokenOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Token19']");
            page.ClickElementByXpath("//a[@href='/Token']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheMobileOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Mobile23']");
            page.ClickElementByXpath("//a[@href='/Mobile']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheSubscriptionOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Subscription25']");
            page.ClickElementByXpath("//a[@href='/Subscription']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheAssetTypeOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Asset Type28']");
            page.ClickElementByXpath("//a[@href='/AssetType']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheIdentityTypeOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Identity Type32']");
            page.ClickElementByXpath("//a[@href='/IdentityType']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheAccountTypeOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Account Type34']");
            page.ClickElementByXpath("//a[@href='/AccountType']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheOpenRoleTypeOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Role Type36']");
            page.ClickElementByXpath("//a[@href='/RoleType']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheSubscriptionTypeOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.WaitUntilElmentVisableByXpath("//a[@id='Subscription Type38']");
            page.ClickElementByXpath("//a[@id='Subscription Type38']");
            page.ClickElementByXpath("//a[@href='/SubscriptionType']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheKensingtonOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Devices']");
            page.ClickElementByXpath("//a[@id='Kensington21']");
            page.ClickElementByXpath("//a[@href='/Kensington']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheAdminOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin47']");
            page.ClickElementByXpath("//a[@href='/Admin']");
            page.WaitOnAddNew();
        }
    }
    public class OpenThePermissionOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Admin']");
            page.ClickElementByXpath("//a[@id='Permissions49']");
            page.ClickElementByXpath("//a[@href='/Permission']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheApplicationOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Application']");
            page.ClickElementByXpath("//a[@id='Application']");
            page.ClickElementByXpath("//a[@id='Application44']");
            page.ClickElementByXpath("//a[@href='/Application']");
            page.WaitOnAddNew();
        }
    }
    public class OpenTheSystemOverviewPage : Task
    {
        public override void PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='System']");
            page.ClickElementByXpath("//a[@id='System']");
            page.ClickElementByXpath("//a[@id='System41']");
            page.ClickElementByXpath("//a[@href='/System']");
            page.WaitOnAddNew();
        }
    }
}
