using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;
using CMDB.UI.Specflow.Abilities.Pages.Types;

namespace CMDB.UI.Specflow.Questions.AssetType
{
    public class OpenTheAssetypeCreatePage : Question<CreateTypePage>
	{
        public override CreateTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AssetTypeOverviewPage>();
            page.ClickElementByXpath(AssetTypeOverviewPage.NewXpath);
            return new();
        }
    }
}
