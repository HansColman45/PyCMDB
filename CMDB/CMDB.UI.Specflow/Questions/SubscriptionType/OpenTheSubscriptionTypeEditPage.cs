using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.SubscriptionType;

namespace CMDB.UI.Specflow.Questions.SubscriptionType
{
    public class OpenTheSubscriptionTypeEditPage : Question<UpdateSubscriptionTypePage>
    {
        public override UpdateSubscriptionTypePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<SubscriptionTypeOverviewPage>();
            page.ClickElementByXpath(Abilities.Pages.MainPage.EditXpath);
            return new();
        }
    }
}
