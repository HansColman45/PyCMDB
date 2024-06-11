using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.SubscriptionType
{
    public class SubscriptionTypeOverviewPage : MainPage
    {
        public SubscriptionTypeOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateSubscriptionTypePage Create()
        {
            ClickElementByXpath(NewXpath);
            return new(WebDriver);
        }
        public SubscriptionTypeDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(WebDriver);
        }
        public UpdateSubscriptionTypePage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(WebDriver);
        }
        public DeactivateSubscriptionTypePage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(WebDriver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
