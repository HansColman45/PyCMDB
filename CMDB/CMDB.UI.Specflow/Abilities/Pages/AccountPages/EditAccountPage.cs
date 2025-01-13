using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AccountPages
{
    public class EditAccountPage : MainPage
    {
        public EditAccountPage() : base()
        {
        }
        public string UserId
        {
            set => EnterInTextboxByXPath("//input[@id='UserID']", value);
            get => TekstFromTextBox("//input[@id='UserID']");
        }
        public string Type
        {
            set => SelectTektInDropDownByXpath("//select[@id='Type_TypeId']", value);
        }
        public string Application
        {
            set => SelectTektInDropDownByXpath("//select[@id='Application_AppID']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
            WaitOnAddNew();
        }
    }
}
