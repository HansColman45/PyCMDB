using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class UpdateSubscriptionTypePage : MainPage
    {
        public UpdateSubscriptionTypePage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string Category
        {
            set => SelectTektInDropDownByXpath("//select[@id='Category']", value);
        }
        public string Provider
        {
            set => EnterInTextboxByXPath("//input[@id='Provider']", value);
        }
        public string Type
        {
            set => EnterInTextboxByXPath("//input[@id='Type']", value);
        }
        public string Description
        {
            set => EnterInTextboxByXPath("//input[@id='Description']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}
