using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Monitor
{
    public class MonitorDetailPage : MainPage
    {
        public MonitorDetailPage() : base()
        {
        }
        public MonitorReleaseIdentityPage ReleaseIdentityPage()
        {
            ClickElementByXpath(ReleaseIdenityXpath);
            return new();
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'screen')]");
        }
    }
}
