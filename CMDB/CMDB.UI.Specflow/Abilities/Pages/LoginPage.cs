using Bright.ScreenPlay.Abilities;

namespace CMDB.UI.Specflow.Abilities.Pages
{
    public class LoginPage : OpenAWebPage
    {
        public LoginPage()
        {
            Settings.BaseUrl = "https://localhost:44314/";
        }
        public string UserId
        {
            set => EnterInTextboxByXPath("//input[@type='text']", value);
        }
        public string Password
        {
            set => EnterInTextboxByXPath("//input[@type='password']", value);
        }
    }
}
