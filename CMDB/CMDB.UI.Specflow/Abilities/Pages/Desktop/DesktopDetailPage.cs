using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Desktop
{
    public class DesktopDetailPage : MainPage
    {
        public DesktopDetailPage() : base()
        {
        }
        public DesktopReleaseIdentityPage ReleaseIdentity()
        {
            ClickElementByXpath(ReleaseIdenityXpath);
            return new();
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'desktop')]");
        }
    }
}
