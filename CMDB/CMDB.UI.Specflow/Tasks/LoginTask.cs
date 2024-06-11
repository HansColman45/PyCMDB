using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Task = Bright.ScreenPlay.Tasks.Task;

namespace CMDB.UI.Specflow.Tasks
{
    public class LoginTask : Task
    {
        public override void PerformAs(IPerformer actor)
        {}
        public static MainPage LoginAs(IPerformer actor,string userName, string password)
        {
            actor.GetAbility<LoginPage>().EnterUserID(userName);
            actor.GetAbility<LoginPage>().EnterPassword(password);
            var mainPage = actor.GetAbility<LoginPage>().LogIn();
            actor.SetAbility(mainPage);
            return mainPage;
        }
    }
}
