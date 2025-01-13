using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Questions
{
    public class OpenTheMainPage : Question<MainPage>
    {
        public override MainPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<LoginPage>();
            page.ClickElementByXpath("//button[@type='submit']");
            page.WaitUntilElmentVisableByXpath("//h1");
            return new();
        }
    }
}
