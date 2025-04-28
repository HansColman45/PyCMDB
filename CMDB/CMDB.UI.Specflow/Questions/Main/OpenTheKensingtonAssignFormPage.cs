using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheKensingtonAssignFormPage : Question<KensingtonAssignFormPage>
    {
        public override KensingtonAssignFormPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.ClickElementByXpath("//button[.='Assign']");
            KensingtonAssignFormPage kensingtonAssignFormPage = WebPageFactory.Create<KensingtonAssignFormPage>(page.WebDriver);
            return kensingtonAssignFormPage;
        }
    }
}
