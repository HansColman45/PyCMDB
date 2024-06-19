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
            get => GetSelectedValueFromDropDownByXpath("//select[@id='Type_TypeID']");
            set => SelectValueInDropDownByXpath("//select[@id='Type_TypeID']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}
