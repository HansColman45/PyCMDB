using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages
{
    public class AssignFormPage : MainPage
    {
        public AssignFormPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string Name
        {
            get => TekstFromElementByXpath("//td[@id='Name']");
        }
        public string UserId
        {
            get => TekstFromElementByXpath("//td[@id='UserId']");
        }
        public string EMail
        {
            get => TekstFromElementByXpath("//td[@id='EMail']");
        }
        public string Language
        {
            get => TekstFromElementByXpath("//td[@id='Language']");
        }
        public string Type
        {
            get => TekstFromElementByXpath("//td[@id='Type']");
        }
        public string State
        {
            get => TekstFromElementByXpath("//td[@id='State']");
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
