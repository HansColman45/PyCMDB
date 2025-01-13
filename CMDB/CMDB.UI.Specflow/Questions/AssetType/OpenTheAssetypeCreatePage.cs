using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;

namespace CMDB.UI.Specflow.Questions.AssetType
{
    public class OpenTheAsseTypeCreatePage : Question<CreateAssetTypePage>
	{
        public override CreateAssetTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AssetTypeOverviewPage>();
            page.ClickElementByXpath(AssetTypeOverviewPage.NewXpath);
            return new();
        }
    }
}
