using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;

namespace CMDB.UI.Specflow.Questions
{
    public class OpenTheMainPage : Question<MainPage>
    {
        public override MainPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LoginPage>();
            page.ClickElementByXpath("//button[@type='submit']");
            page.WaitUntilElmentVisableByXpath("//h1");
            return new(page.WebDriver);
        }
    }
}
