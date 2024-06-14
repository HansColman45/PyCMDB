using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks
{
    public class TheMainPageTasks : Task
    {
        public override void PerformAs(IPerformer actor){ }
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
    }
}
