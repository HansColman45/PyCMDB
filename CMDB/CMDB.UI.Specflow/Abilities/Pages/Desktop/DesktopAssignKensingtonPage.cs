using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Desktop
{
    public class DesktopAssignKensingtonPage : MainPage
    {
        public DesktopAssignKensingtonPage(IWebDriver web) : base(web)
        {

        }
        public void SelectKensington(Domain.Entities.Kensington key)
        {
            SelectValueInDropDownByXpath("//select[@id='Kensington']", $"{key.KeyID}");
        }
    }
}
