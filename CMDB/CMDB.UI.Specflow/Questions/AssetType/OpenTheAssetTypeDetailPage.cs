using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.AssetType
{
    public class OpenTheAssetTypeDetailPage : Question<AssetTypeDetailPage>
    {
        public override AssetTypeDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AssetTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            AssetTypeDetailPage assetTypeDetail = WebPageFactory.Create<AssetTypeDetailPage>(page.WebDriver);
            return assetTypeDetail;
        }
    }
}
