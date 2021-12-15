using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class DeactivateAccountPage : MainPage
    {
        public DeactivateAccountPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string UserId 
        { 
            get => TekstFromElementByXpath("//td[@id='UserId']"); 
        }
        public string Type
        {
            get => TekstFromElementByXpath("//td[@id='Type']");
        }
        public string Application
        {
            get => TekstFromElementByXpath("//td[@id='Application']");
        }
        public string State
        {
            get => TekstFromElementByXpath("//td[@id='State']");
        }
        public string Reason
        {
            set => EnterInTextboxByXPath("//input[@id='reason']", value);
        }
        public void Delete()
        {
            ClickElementByXpath("//button[.='Delete']");
            WaitOnAddNew();
        }
    }
}
