using CMDB.Domain.Entities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class AssignAccountPage : MainPage
    {
        public AssignAccountPage() : base()
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
        public void SelectAccount(Account account)
        {
            SelectValueInDropDownByXpath("//select[@id='Account']", account.AccID.ToString());
        }
        public DateTime ValidFrom
        {
            set => EnterDateTimeByXPath("//input[@id='ValidFrom']", value);
        }
        public DateTime ValidUntil
        {
            set => EnterDateTimeByXPath("//input[@id='ValidUntil']", value);
        }
        public AssignFormPage Assign()
        {
            ClickElementByXpath("//button[@type='submit']");
            return new();
        }
    }
}
