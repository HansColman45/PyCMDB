using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks
{
    public class TheMainPageTasks : Task
    {
        public override void PerformAs(IPerformer actor){ }
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
