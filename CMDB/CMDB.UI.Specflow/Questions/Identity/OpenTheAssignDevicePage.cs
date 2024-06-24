using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Identity
{
    public class OpenTheAssignDevicePage : Question<AssignDevicePage>
    {
        public override AssignDevicePage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityOverviewPage>();
            page.ClickElementByXpath("//a[@title='Assign Device']");
            page.WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new();
        }
    }
}
