using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Docking
{
    public class UpdateDockingPage : MainPage
    {
        public UpdateDockingPage() : base()
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
            set => SelectValueInDropDownByXpath("//select[@id='AssetType_TypeID']", value);
            get => GetSelectedValueFromDropDownByXpath("//select[@id='AssetType_TypeID']");
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}
