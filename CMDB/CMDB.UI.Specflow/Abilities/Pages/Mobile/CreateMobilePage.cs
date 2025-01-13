using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Mobile
{
    public class CreateMobilePage : MainPage
    {
        public CreateMobilePage() : base()
        {
        }
        public string IMEI
        {
            set => EnterInTextboxByXPath("//input[@id='IMEI']", value);
        }
        public string Type
        {
            set => SelectValueInDropDownByXpath("//select[@id='MobileType']", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
