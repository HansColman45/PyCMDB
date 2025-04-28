using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Main
{
    /// <summary>
    /// This class will be used to open the AssignForm
    /// </summary>
    public class OpenTheAssignFormPage : Question<AssignFormPage>
    {
        public override AssignFormPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.ClickElementByXpath("//button[.='Assign']");
            AssignFormPage assignFormPage = WebPageFactory.Create<AssignFormPage>(page.WebDriver);
            return assignFormPage;
        }
    }
}
