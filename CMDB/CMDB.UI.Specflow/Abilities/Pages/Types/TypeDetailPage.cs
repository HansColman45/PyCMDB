using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Types
{
    public class TypeDetailPage : MainPage
    {
        public TypeDetailPage() : base()
        {
        }
        public string GetLastLog(string type)
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath($"//td[contains(text(),'{type}')]");
        }
    }
}
