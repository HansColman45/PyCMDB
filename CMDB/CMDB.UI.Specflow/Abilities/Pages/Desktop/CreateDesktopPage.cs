using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Desktop
{
    public class CreateDesktopPage : MainPage
    {
        public CreateDesktopPage() : base()
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
            set => SelectValueInDropDownByXpath("//select[@id='AssetType']", value);
        }
        public string RAM
        {
            set => SelectTektInDropDownByXpath("//select[@id='RAM']", value);
        }
        public string MAC
        {
            set => EnterInTextboxByXPath("//input[@id='MAC']", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
