using CMDB.Domain.Entities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class AssignDevicePage : MainPage
    {
        public AssignDevicePage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public void ClickDevice(Device device)
        {
            ClickElementByXpath($"//*[@id='{device.AssetTag}']");
        }
        public AssignFormPage Assign()
        {
            ClickElementByXpath("//button[@type='submit']");
            return new(WebDriver);
        }
    }
}
