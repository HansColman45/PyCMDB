using CMDB.Domain.Entities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Kensington
{
    public class KensingtonAssignDevicePage : MainPage
    {
        public KensingtonAssignDevicePage(IWebDriver web) : base(web)
        {
        }
        public void SelectDevice(Device device)
        {
            SelectValueInDropDownByXpath("//select[@id='Device']", $"{device.AssetTag}");
        }
    }
}
