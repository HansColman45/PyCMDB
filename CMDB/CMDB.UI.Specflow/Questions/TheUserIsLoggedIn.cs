using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace CMDB.UI.Specflow.Questions
{
    public class TheUserIsLoggedIn : Question<bool>
    {
        public override bool PerformAs(IPerformer actor)
        {
            return actor.GetAbility<MainPage>().IsLoggedIn;
        }
        public static bool IsLoggedInAs(IPerformer actor)
        {
            return actor.GetAbility<MainPage>().IsLoggedIn;
        }
    }
}
