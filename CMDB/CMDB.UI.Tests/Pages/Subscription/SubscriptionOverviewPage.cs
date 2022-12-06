using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    internal class SubscriptionOverviewPage : MainPage
    {
        public SubscriptionOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateSubscriptionPage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public SubscriptionDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public UpdateSubscriptionPage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateSubscriptionPage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public SubscriptionAssignIdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(driver);
        }
    }
}
