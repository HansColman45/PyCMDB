using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Monitor
{
    public class UpdateMonitorPage : MainPage
    {
        public UpdateMonitorPage(IWebDriver web) : base(web)
        {
        }
        public string AssetTag
        {
            get => TekstFromTextBox("//input[@id='AssetTag']");
        }
        public string SerialNumber
        {
            get => TekstFromTextBox("//input[@id='SerialNumber']");
            set => EnterInTextboxByXPath("//input[@id='SerialNumber']", value);
        }
        public string Type
        {
            get => GetSelectedValueFromDropDownByXpath("//select[@id='AssetType_TypeID']");
            set => SelectValueInDropDownByXpath("//select[@id='AssetType_TypeID']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}
