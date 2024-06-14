using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Types
{
    public class TypeDetailPage : MainPage
    {
        public TypeDetailPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string GetLastLog(string type)
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath($"//td[contains(text(),'{type}')]");
        }
    }
}
