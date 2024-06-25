using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Abilities.Pages.Types;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenAssetCategoryOverview : Question<AssetCategoryOverviewPage>
    {
        public override AssetCategoryOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Asset Category30']");
            page.ClickElementByXpath("//a[@href='/AssetCategory']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
