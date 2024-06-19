using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Monitor
{
    public class CreateMonitorPage : MainPage
    {
        public CreateMonitorPage() : base()
        {
        }
        public string AssetTag
        {
            set => EnterInTextboxByXPath("//input[@id='AssetTag']", value);
        }
        public string SerialNumber
        {
            set => EnterInTextboxByXPath("//input[@id='SerialNumber']", value);
        }
        public string Type
        {
            set => SelectValueInDropDownByXpath("//select[@id='Type']", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
