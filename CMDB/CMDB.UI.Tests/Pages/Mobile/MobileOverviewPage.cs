using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class MobileOverviewPage : MainPage
    {
        public MobileOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateMobilePage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public MobileDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public UpdateMobilePage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateMobilePage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public MobileAssignIdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(driver);
        }
    }
}
