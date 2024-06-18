using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;

namespace CMDB.UI.Specflow.Questions.AssetType
{
    public class OpenTheAssetTpeCreatePage : Question<CreateAssetTypePage>
    {
        public override CreateAssetTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AssetTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheAssetTypeDetailPage : Question<AssetTypeDetailPage>
    {
        public override AssetTypeDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AssetTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.InfoXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheAssetTypeEditPage : Question<UpdateAssetTypePage>
    {
        public override UpdateAssetTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AssetTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            return new(page.WebDriver);
        }
    }
    public class OpenTheAssetTypeDeactivatePage : Question<DeactivateAssetTypePage>
    {
        public override DeactivateAssetTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AssetTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.DeactivateXpath);
            return new(page.WebDriver);
        }
    }
}
