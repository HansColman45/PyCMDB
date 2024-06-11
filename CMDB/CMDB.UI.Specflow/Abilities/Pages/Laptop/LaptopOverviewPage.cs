using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Laptop
{
    public class LaptopOverviewPage : MainPage
    {
        public LaptopOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateLaptopPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(WebDriver);
        }
        public LaptopDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(WebDriver);
        }
        public UpdateLaptopPage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(WebDriver);
        }
        public DeactivateLaptopPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(WebDriver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public LaptopAssignIdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(WebDriver);
        }
    }
}
