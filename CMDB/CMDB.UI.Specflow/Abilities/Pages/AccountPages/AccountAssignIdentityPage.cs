using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AccountPages
{
    public class AccountAssignIdentityPage : MainPage
    {
        public AccountAssignIdentityPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public void SelectIdentity(Domain.Entities.Identity identity)
        {
            SelectValueInDropDownByXpath("//select[@id='Identity']", identity.IdenId.ToString());
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
