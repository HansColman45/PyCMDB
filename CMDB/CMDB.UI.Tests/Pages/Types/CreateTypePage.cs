using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class CreateTypePage : MainPage
    {
        public CreateTypePage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string Type
        {
            set => EnterInTextboxByXPath("//input[@id='Type']", value);
        }
        public string Description
        {
            set => EnterInTextboxByXPath("//input[@id='Description']", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
