using CMDB.Domain.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class AssignAccountPage : MainPage
    {
        public AssignAccountPage() : base()
        {
        }
        public string Name
        {
            get => TekstFromElementByXpath("//td[@id='Name']");
        }
        public string UserId
        {
            get => TekstFromElementByXpath("//td[@id='UserId']");
        }
        public string EMail
        {
            get => TekstFromElementByXpath("//td[@id='EMail']");
        }
        public string Language
        {
            get => TekstFromElementByXpath("//td[@id='Language']");
        }
        public string Type
        {
            get => TekstFromElementByXpath("//td[@id='Type']");
        }
        public string State
        {
            get => TekstFromElementByXpath("//td[@id='State']");
        }
        public void SelectAccount(Account account)
        {
            SelectValueInDropDownByXpath("//select[@id='Account']", account.AccID.ToString());
        }
        public DateTime ValidFrom
        {
            set => EnterDateTimeByXPath("//input[@id='ValidFrom']", value);
        }
        public DateTime ValidUntil
        {
            set => EnterDateTimeByXPath("//input[@id='ValidUntil']", value);
        }

        private new void EnterDateTimeByXPath(string xPath, DateTime dateTime)
        {
            log.Debug($"Enter date: {dateTime:dd-MM-yyyy HH:mm} using xpath: {xPath}");
            Thread.Sleep(settings.ShortTimeOutInMilSec);
            WebDriverWait webDriverWait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(settings.LongTimeOutInSec))
            {
                PollingInterval = TimeSpan.FromMilliseconds(settings.PollingIntervallInMilSec)
            };
            webDriverWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            webDriverWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement webElement = webDriverWait.Until((IWebDriver x) => x.FindElement(By.XPath(xPath)));
            webElement.Click();
            webElement.Clear();
            webElement.SendKeys($"{dateTime:MMddyyyy}");
            //SendTab(By.XPath(xPath));
            webElement.SendKeys($"{dateTime:hh:mm}");
            webElement.SendKeys($"{dateTime:tt}");
        }
    }
}
