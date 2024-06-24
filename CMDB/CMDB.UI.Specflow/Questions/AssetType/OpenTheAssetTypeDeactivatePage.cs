using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.AssetType
{
    public class OpenTheAssetTypeDeactivatePage : Question<OpenTheAssetTypeDeactivatePage>
    {
        public override OpenTheAssetTypeDeactivatePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AssetTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            return new();
        }
    }
}
