using Bright.ScreenPlay.Abilities;
using Bright.ScreenPlay.Settings;

namespace CMDB.UI.Specflow.Abilities.Pages
{
    public class LoginPage : OpenAWebPage
    {
        public LoginPage()
        {
            WebSettings settings = new()
            {
                BaseUrl = "https://localhost:5001/"
            };
            Settings = settings;
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
