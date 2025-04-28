using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;

namespace CMDB.UI.Specflow.Questions.Account
{
    public class OpenTheAccountAssignFormPage : Question<AssignFormPage>
    {
        public override AssignFormPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.ClickElementByXpath("//button[@type='submit']");
            AssignFormPage assignPage = WebPageFactory.Create<AssignFormPage>(page.WebDriver);
            return assignPage;
        }
    }
}
