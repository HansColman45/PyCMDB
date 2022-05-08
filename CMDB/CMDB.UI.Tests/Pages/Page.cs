using CMDB.Testing.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace CMDB.UI.Tests.Pages
{
    public class Page
    {
        /// <summary>
        /// This is the NLog property
        /// </summary>
        protected readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// This is the WebDriver property
        /// </summary>
        protected IWebDriver driver;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="webDriver">The web driver</param>
        public Page(IWebDriver webDriver)
        {
            driver = webDriver;
        }
        /// <summary>
        /// This function will click an Element using XPath
        /// </summary>
        /// <param name="xpath">The xpath</param>
        protected void ClickElementByXpath(string xpath)
        {
            log.Debug("Clicking ellement by xpath: {0}", xpath);
            Thread.Sleep(50);
            try
            {
                var fluentWait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
                fluentWait.IgnoreExceptionTypes(typeof(ElementNotInteractableException));
                try
                {
                    IWebElement element = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
                    element.Click();
                }
                catch (Exception)
                {
                    throw;
                }
                Thread.Sleep(700);
            }
            catch (StaleElementReferenceException ex)
            {
                log.Error(ex);
                IWebElement element = driver.FindElement(By.XPath(xpath));
                element.Click();
            }
        }
        /// <summary>
        /// This function will click an ellement using CSS
        /// </summary>
        /// <param name="CSS">The CSS</param>
        protected void ClickElementByCSS(string CSS)
        {
            log.Debug("Clicking ellement by css: {0}", CSS);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            IWebElement element = wait.Until(Condition(By.CssSelector(CSS)));
            element.Click();
            Thread.Sleep(700);
        }
        /// <summary>
        /// This function will enter a given text in a TextBox using the Xpath
        /// </summary>
        /// <param name="xPath">The XPath</param>
        /// <param name="textToEnter">The text to enter</param>
        protected void EnterInTextboxByXPath(string xPath, string textToEnter)
        {
            log.Debug("Enter in ellement by xpath: {0}, {1}", xPath, textToEnter);
            try
            {
                Thread.Sleep(500);
                var fluentWait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
                fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
                IWebElement element = fluentWait.Until(x => x.FindElement(By.XPath(xPath)));
                element.Click();
                element.Clear();
                element.SendKeys(textToEnter);
                Thread.Sleep(500);
            }
            catch (StaleElementReferenceException ex)
            {
                log.Error(ex.ToString());
                IWebElement element = driver.FindElement(By.XPath(xPath));
                element.Click();
                element.SendKeys(textToEnter);
                Thread.Sleep(500);
            }
        }
        protected void EnterDateTimeByXPath(string xPath, DateTime dateTime)
        {
            Thread.Sleep(500);
            var fluentWait = new WebDriverWait(driver, TimeSpan.FromSeconds(100))
            {
                PollingInterval = TimeSpan.FromMilliseconds(150)
            };
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement element = fluentWait.Until(x => x.FindElement(By.XPath(xPath)));
            element.Click();
            element.Clear();
            element.SendKeys(dateTime.ToString("ddMMyyyy"));
            SendTab(By.XPath(xPath));
            element.SendKeys(dateTime.ToString("hh:mm"));
            element.SendKeys(dateTime.ToString("tt"));
        }
        protected void SendTab(By by)
        {
            IWebElement element = driver.FindElement(by);
            element.SendKeys(Keys.Tab);
        }
        /// <summary>
        /// This function will select an option from a dropdown using the value
        /// </summary>
        /// <param name="xPath">The Xpath</param>
        /// <param name="value">The value to select</param>
        protected void SelectValueInDropDownByXpath(string xPath, string value)
        {
            log.Debug("Select value in dropdown by xpath: {0}, {1}", xPath, value);
            Thread.Sleep(500);
            var fluentWait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement element = fluentWait.Until(x => x.FindElement(By.XPath(xPath)));
            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);
        }
        /// <summary>
        /// This function will select an option from a dropdown using the text
        /// </summary>
        /// <param name="xPath">The Xpath</param>
        /// <param name="text">The text to select</param>
        protected void SelectTektInDropDownByXpath(string xPath, string text)
        {
            log.Debug("Select value in dropdown by xpath: {0}, {1}", xPath, text);
            Thread.Sleep(500);
            var fluentWait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement element = fluentWait.Until(x => x.FindElement(By.XPath(xPath)));
            var selectElement = new SelectElement(element);
            selectElement.SelectByText(text, true);
        }
        /// <summary>
        /// This function will enter a given text in a TextBox using CSS
        /// </summary>
        /// <param name="CSS">The CSS</param>
        /// <param name="textToEnter">The text to enter</param>
        protected void EnterInTextboxByCSS(string CSS, string textToEnter)
        {
            log.Debug("Enter in ellement by CSS: {0}, {1}", CSS, textToEnter);
            var fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(5),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement element = fluentWait.Until(Condition(By.CssSelector(CSS)));
            element.Click();
            element.SendKeys(textToEnter);
            Thread.Sleep(500);
        }
        /// <summary>
        /// This function will return the text from a WebElement selected by XPath
        /// </summary>
        /// <param name="xpath">The XPath</param>
        /// <returns>The text from the element</returns>
        protected string TekstFromElementByXpath(string xpath)
        {
            log.Debug("Get tekst from ellement by xpath: {0}", xpath);
            var fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(10),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement element = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
            return element.Text;
        }
        /// <summary>
        /// This function will get the text from a textbox
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        protected string TekstFromTextBox(string xpath)
        {
            log.Debug("Get tekst from ellement by xpath: {0}", xpath);
            var fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(10),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement element = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
            return element.GetAttribute("value");
        }
        protected string GetSelectedValueFromDropDownByXpath(string xpath)
        {
            log.Debug("Get selected value from Dropdown by xpath: {0}", xpath);
            var fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(10),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement element = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
            SelectElement selectElement = new(element);
            return selectElement.SelectedOption.Text;
        }
        /// <summary>
        /// This function will return the text from a WebElement selected by CSS
        /// </summary>
        /// <param name="css">The CSS</param>
        /// <returns>The text from the element</returns>
        protected string TekstFromElementByCss(string css)
        {
            log.Debug("Get tekst from ellement by css: {0}", css);
            var fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(10),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement element = fluentWait.Until(Condition(By.CssSelector(css)));
            return element.Text;
        }
        /// <summary>
        /// This function will wait until an element is visable using XPath
        /// </summary>
        /// <param name="xptah">The xpath</param>
        protected void WaitUntilElmentVisableByXpath(string xptah)
        {
            log.Debug("Wait Until Element Vissable by Xpath: {0}", xptah);
            var fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(5),
                PollingInterval = TimeSpan.FromSeconds(250)
            };
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            fluentWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath(xptah)));
        }
        /// <summary>
        /// This function will wait until an element is visable using CSS
        /// </summary>
        /// <param name="CSS">The CSS</param>
        protected void WaitUntilElmentVisableByCSS(string CSS)
        {
            log.Debug("Wait Until Element Vissable by CSS: {0}", CSS);
            var fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(5),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            fluentWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(CSS)));
        }
        /// <summary>
        /// This function will return true if the element is visable and and enabled
        /// </summary>
        /// <param name="by">The way you want to select the element</param>
        /// <returns>bool</returns>
        protected bool IsElementVisable(By by)
        {
            try
            {
                object p = driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                var element = driver.FindElement(by);
                return element.Displayed && element.Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        /// <summary>
        /// This function will return the value of a property for an Element found by XPath
        /// </summary>
        /// <param name="xpath">The xpath</param>
        /// <param name="property">The property</param>
        /// <returns></returns>
        protected string GetAttributeFromXpath(string xpath, string property)
        {
            log.Debug("Get property {0} from ellement by xpath: {1}", property, xpath);
            var fluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(10),
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
            IWebElement element = fluentWait.Until(x => x.FindElement(By.XPath(xpath)));
            return element.GetAttribute(property);
        }
        public void TakeScreenShot(string step)
        {
            if (Settings.TakeScreenShot) { 
                ITakesScreenshot takesScreenshot = (ITakesScreenshot)driver;
                var screenshot = takesScreenshot.GetScreenshot();
                var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");
                string fileName = $"{step}_{DateTime.Now:yyyy-MM-dd'T'HH-mm-ss}.png";
                string tempFileName = Path.Combine(path, @"../../../Screenshots/", fileName);

                screenshot.SaveAsFile(tempFileName, ScreenshotImageFormat.Png);
                log.Debug("Screenshot saved: {0}", tempFileName);
            }
        }
        /// <summary>
        /// This function will scoll to an given ellemt 
        /// </summary>
        /// <param name="by">the way you want to select the element</param>
        public void ScrollToElement(By by)
        {
            Thread.Sleep(2000);
            IWebElement s = driver.FindElement(by);
            IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
            je.ExecuteScript("arguments[0].scrollIntoView(false);", s);
        }
        private static Func<IWebDriver, IWebElement> Condition(By locator)
        {
            return (driver) =>
            {
                try
                {
                    var element = driver.FindElements(locator).FirstOrDefault();
                    return element != null && element.Displayed && element.Enabled ? element : null;
                }
                catch (WebDriverException)
                {
                    throw;
                }
            };
        }
    }
}
