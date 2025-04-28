using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Laptop
{
    public class LaptopReleaseIdentityPage : MainPage
    {
        public LaptopReleaseIdentityPage(IWebDriver web) : base(web)
        {
        }
        public string UserId => GetAttributeFromXpath("//td[@id='UserId']", "innerHTML");
        public string Type => GetAttributeFromXpath("//td[@id='Type']", "innerHTML");
        public string Application => GetAttributeFromXpath("//td[@Application]", "innerHTML");
        public string IdentityName => GetAttributeFromXpath("//td[@id='IdentityName']", "innerHTML");
        public string IdentityUserId => GetAttributeFromXpath("//td[@id='IdentityUserId']", "innerHTML");
        public string IdentityEMail => GetAttributeFromXpath("//td[@id='IdentityEMail']", "innerHTML");

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
