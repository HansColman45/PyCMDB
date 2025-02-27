using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Keys
{
    class OpenTheKensingtonCreatePage: Question<KensingtonCreatePage>
    {
        public override KensingtonCreatePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<KensingtonOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.NewXpath);
            return new();
        }
    }
}
