using CMDB.Domain.Entities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
