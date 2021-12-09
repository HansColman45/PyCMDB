using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class LaptopOverviewPage : MainPage
    {
        public LaptopOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateLaptopPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public LaptopDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public UpdateLaptopPage Update()
        {
            ClickElementByXpath(EditXpath);
            WaitUntilElmentVisableByXpath("//input[@name='FirstName']");
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
