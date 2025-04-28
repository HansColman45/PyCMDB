using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Laptop
{
    public class LaptopAssignKensingtonPage : MainPage
    {
        public LaptopAssignKensingtonPage(IWebDriver web) : base(web)
        {
        }
        public void SelectKensington(Domain.Entities.Kensington key)
        {
            SelectValueInDropDownByXpath("//select[@id='Kensington']", $"{key.KeyID}");
        }
    }
}
