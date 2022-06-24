using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class TypeOverviewPage : MainPage
    {
        public TypeOverviewPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public CreateTypePage New()
        {
            ClickElementByXpath(NewXpath);
            return new(driver);
        }
        public TypeDetailPage Detail()
        {
            ClickElementByXpath(InfoXpath);
            return new(driver);
        }
        public UpdateTypePage Update()
        {
            ClickElementByXpath(EditXpath);
            return new(driver);
        }
        public DeactivateTypePage Deactivate()
        {
            ClickElementByXpath(DeactivateXpath);
            return new(driver);
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
        public TypeAssignIdentityPage AssignIdentity()
        {
            ClickElementByXpath(AssignIdenityXpath);
            WaitUntilElmentVisableByXpath("//button[@type='submit']");
            return new(driver);
        }
    }
}
