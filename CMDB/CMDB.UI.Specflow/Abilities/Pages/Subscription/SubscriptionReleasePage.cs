using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Subscription
{
    public class SubscriptionReleasePage :MainPage
    {
        public SubscriptionReleasePage(IWebDriver web) : base(web)
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
