using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenAssetTypeOverviewPage : Question<AssetTypeOverviewPage>
    {
        public override AssetTypeOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Asset Type28']");
            page.ClickElementByXpath("//a[@href='/AssetType']");
            page.WaitOnAddNew();
            AssetTypeOverviewPage assetTypeOverviewPage = WebPageFactory.Create<AssetTypeOverviewPage>(page.WebDriver);
            return assetTypeOverviewPage;
        }
    }
}
