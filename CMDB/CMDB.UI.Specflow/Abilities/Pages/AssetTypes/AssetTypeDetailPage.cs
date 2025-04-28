using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AssetTypes
{
    public class AssetTypeDetailPage : MainPage
    {
        public AssetTypeDetailPage(IWebDriver web) : base(web)
        {
        }
        public int Id
        {
            get
            {
                string id = GetAttributeFromXpath("//td[@id='Id']", "innerHTML");
                return int.Parse(id);
            }
        }
        public string GetLastLog()
        {
            ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return TekstFromElementByXpath("//td[contains(text(),'assettype')]");
        }
    }
}
