using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class UpdateIdentityPage : MainPage
    {
        public UpdateIdentityPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string FirstName
        {
            set => EnterInTextboxByXPath("//input[@name='FirstName']", value);
            get => TekstFromTextBox("//input[@name='FirstName']");
        }
        public string LastName
        {
            set => EnterInTextboxByXPath("//input[@name='LastName']", value);
            get => TekstFromTextBox("//input[@name='LastName']");
        }
        public string Email
        {
            set => EnterInTextboxByXPath("//input[@name='EMail']", value);
            get => TekstFromTextBox("//input[@name='EMail']");
        }
        public string Language
        {
            set => SelectValueInDropDownByXpath("//select[@id='Language']", value);
        }
        public string Company
        {
            set => EnterInTextboxByXPath("//input[@name='Company']", value);
            get => TekstFromTextBox("//input[@name='Company']");
        }
        public string Type
        {
            set => SelectTektInDropDownByXpath("//select[@id='Type']", value);
        }
        public string UserId
        {
            set => EnterInTextboxByXPath("//input[@name='UserID']", value);
            get => TekstFromTextBox("//input[@name='UserID']");
        }
        public void Update()
        {
            ClickElementByXpath("//button[.='Update']");
        }
    }
}
