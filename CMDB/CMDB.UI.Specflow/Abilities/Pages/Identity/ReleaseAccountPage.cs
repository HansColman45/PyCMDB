using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class ReleaseAccountPage : MainPage
    {
        public ReleaseAccountPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string Employee
        {
            set => EnterInTextboxByXPath("//input[@id='Employee']", value);
            get => TekstFromTextBox("//input[@id='Employee']");
        }
        public string ITEmployee
        {
            set => EnterInTextboxByXPath("//input[@id='ITEmp']", value);
            get => TekstFromTextBox("//input[@id='ITEmp']");
        }
        public void CreatePDF()
        {
            ClickElementByXpath("//button[@type='submit']");
            WaitOnAddNew();
        }
    }
}
