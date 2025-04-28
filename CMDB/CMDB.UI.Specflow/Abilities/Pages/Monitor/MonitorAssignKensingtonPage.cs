using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Monitor
{
    public class MonitorAssignKensingtonPage: MainPage
    {
        public MonitorAssignKensingtonPage(IWebDriver web) : base(web)
        {
        }
        public void SelectKensington(Domain.Entities.Kensington key)
        {
            SelectValueInDropDownByXpath("//select[@id='Kensington']", $"{key.KeyID}");
        }
    }
}
