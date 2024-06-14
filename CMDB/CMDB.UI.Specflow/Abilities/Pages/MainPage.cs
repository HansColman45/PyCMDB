using Bright.ScreenPlay.Abilities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages
{
    public class MainPage : OpenAWebPage
    {
        public MainPage()
        {
        }
        public MainPage(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }
        public static string NewXpath => "//a[.=' Add']";
        public static string EditXpath => "//a[@title='Edit']";
        public static string DeactivateXpath => "//a[@title='Deactivate']";
        public static string InfoXpath => "//a[@title='Info']";
        public static string ActivateXpath => "//a[@title='Activate']";
        public static string AssignIdenityXpath => "//a[@title='Assign Identity']";
        public static string ReleaseIdenityXpath => "//a[@title='Release Identity']";
        public static string ReleaseDeviceXPath => "//a[@id='ReleaseDevice']";
        public static string ReleaseAccountXPath => "//a[@id='ReleaseAccount']";
        public string Title => GetAttributeFromXpath("//h2", "innerHTML");
        public bool IsLoggedIn => IsElementVisable(By.XPath("//h1"));

        public void Search(string searchstring)
        {
            EnterInTextboxByXPath("//input[@name='search']", searchstring);
            ClickElementByXpath("//button[@type='submit']");
        }

        public new void Dispose()
        {
            WebDriver.Quit();
            WebDriver.Close();
        }
        public void WaitOnAddNew()
        {
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
