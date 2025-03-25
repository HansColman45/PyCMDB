using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;

namespace CMDB.UI.Specflow.Questions.Keys
{
    public class OpenTheKensingtonReleaseDevicePage : Question<KensingtonReleaseDevicePage>
    {
        public override KensingtonReleaseDevicePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<KensingtonDetailPage>();
            page.ClickElementByXpath(KensingtonReleaseDevicePage.ReleaseDeviceXPath);
            return new();
        }
    }
}
