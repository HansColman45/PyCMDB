using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class SubscriptionTypeOverviewPage : MainPage
    {
        public SubscriptionTypeOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateSubscriptionTypePage Create()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public SubscriptionTypeDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public UpdateSubscriptionTypePage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateSubscriptionTypePage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
