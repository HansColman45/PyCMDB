using Bright.ScreenPlay.Abilities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages
{
    public class MainPage : OpenAWebPage
    {
        public MainPage(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }
        protected static string NewXpath => "//a[.=' Add']";
        protected static string EditXpath => "//a[@title='Edit']";
        protected static string DeactivateXpath => "//a[@title='Deactivate']";
        protected static string InfoXpath => "//a[@title='Info']";
        protected static string ActivateXpath => "//a[@title='Activate']";
        protected static string AssignIdenityXpath => "//a[@title='Assign Identity']";
        protected static string ReleaseIdenityXpath => "//a[@title='Release Identity']";
        public static string ReleaseDeviceXPath => "//a[@id='ReleaseDevice']";
        public static string ReleaseAccountXPath => "//a[@id='ReleaseAccount']";
        public string Title => GetAttributeFromXpath("//h2", "innerHTML");
        public bool IsLoggedIn => IsElementVisable(By.XPath("//h1"));

        public new void Dispose()
        {
            WebDriver.Quit();
            WebDriver.Close();
        }
    }
}
