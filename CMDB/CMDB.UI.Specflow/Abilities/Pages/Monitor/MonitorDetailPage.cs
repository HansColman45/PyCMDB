using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Monitor
{
    public class MonitorDetailPage : MainPage
    {
        public MonitorDetailPage(IWebDriver web) : base(web)
        {
        }
        public MonitorReleaseIdentityPage ReleaseIdentityPage()
        {
            ClickElementByXpath(ReleaseIdenityXpath);
            return new(WebDriver);
        }
        public string LastLogLine
        {
            get
            {
                ScrollToElement(By.XPath("//h3[.='Log overview']"));
                return TekstFromElementByXpath("//td[contains(text(),'screen')]");
            }
        }
    }
}
