using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Identity;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheReleaseMobilePage : Question<ReleaseMobilePage>
    {
        public override ReleaseMobilePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityDetailPage>();
            page.ClickElementByXpath(IdentityDetailPage.ReleaseMobileXPath);
            return new();
        }
    }
}
