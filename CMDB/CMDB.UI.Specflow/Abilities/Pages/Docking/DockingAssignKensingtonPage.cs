using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Docking
{
    public class DockingAssignKensingtonPage: MainPage
    {
        public DockingAssignKensingtonPage(IWebDriver web) : base(web)
        {
            
        }
        public void SelectKensington(Domain.Entities.Kensington key)
        {
            SelectValueInDropDownByXpath("//select[@id='Kensington']", $"{key.KeyID}");
        }
    }
}
