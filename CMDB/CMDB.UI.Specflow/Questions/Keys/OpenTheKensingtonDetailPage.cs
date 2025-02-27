using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;

namespace CMDB.UI.Specflow.Questions.Keys
{
    class OpenTheKensingtonDetailPage : Question<KensingtonDetailPage>
    {
        public override KensingtonDetailPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<KensingtonOverviewPage>();
            page.ClickElementByXpath(KensingtonOverviewPage.InfoXpath);
            return new();
        }
    }
}
