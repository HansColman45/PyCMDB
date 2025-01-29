using Bright.ScreenPlay.Abilities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages
{
    public class MainPage : OpenAWebPage
    {
        public MainPage()
        {
        }
        /// <summary>
        /// Xpath for the Add button
        /// </summary>
        public static string NewXpath => "//a[.=' Add']";
        /// <summary>
        /// Xpath for the Edit button
        /// </summary>
        public static string EditXpath => "//a[@title='Edit']";
        /// <summary>
        /// Xpath for the Deactivate button
        /// </summary>
        public static string DeactivateXpath => "//a[@title='Deactivate']";
        /// <summary>
        /// Xpath for the Info button
        /// </summary>
        public static string InfoXpath => "//a[@title='Info']";
        /// <summary>
        /// Xpath for the Activate button
        /// </summary>
        public static string ActivateXpath => "//a[@title='Activate']";
        /// <summary>
        /// Xpath for the Assign Identity button
        /// </summary>
        public static string AssignIdenityXpath => "//a[@title='Assign Identity']";
        /// <summary>
        /// This is the XPath for the assign Subscription
        /// </summary>
        public static string AssignSubscriptionXpath => "//a[@title='Assign Subscription']";
        /// <summary>
        /// Xpath for the Release Identity button
        /// </summary>
        public static string ReleaseIdenityXpath => "//a[@title='Release Identity']";
        /// <summary>
        /// Xpath for the Release Device button
        /// </summary>
        public static string ReleaseDeviceXPath => "//a[@id='ReleaseDevice']";
        /// <summary>
        /// Xpath for the Release Mobile button
        /// </summary>
        public static string ReleaseMobileXPath => "//a[@id='ReleaseMobile']";
        /// <summary>
        /// Xpath for the Release Subscritpion button
        /// </summary>
        public static string ReleaseInternetSubscriptionXPath => "//a[@id='ReleaseInternetSubscription']";
        public static string ReleaseSubscriptionXpath => "//a[@id='ReleaseSubscription']";
        /// <summary>
        /// Xpath for the Release Account button
        /// </summary>
        public static string ReleaseAccountXPath => "//a[@id='ReleaseAccount']";
        /// <summary>
        /// Xpath for the Title
        /// </summary>
        public string Title => GetAttributeFromXpath("//h2", "innerHTML");
        /// <summary>
        /// Xpath for the Log overview
        /// </summary>
        public static string LogOverviewXpath => "//h3[.='Log overview']";
        /// <summary>
        /// Wether the user is logged in
        /// </summary>
        public bool IsLoggedIn => IsElementVisable(By.XPath("//h1"));
        
        public void Search(string searchstring)
        {
            EnterInTextboxByXPath("//input[@name='search']", searchstring);
            ClickElementByXpath("//button[@type='submit']");
        }
        public void WaitOnAddNew()
        {
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
