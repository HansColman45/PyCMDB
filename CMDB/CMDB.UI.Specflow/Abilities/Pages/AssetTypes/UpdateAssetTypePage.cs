using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AssetTypes
{
    public class UpdateAssetTypePage : MainPage
    {
        public UpdateAssetTypePage() : base()
        {
        }
        public string Category
        {
            set => SelectTektInDropDownByXpath("//select[@id='Category']", value);
        }
        public string Vendor
        {
            set => EnterInTextboxByXPath("//input[@id='Vendor']", value);
            get => TekstFromTextBox("//input[@id='Vendor']");
        }
        public string Type
        {
            set => EnterInTextboxByXPath("//input[@id='Type']", value);
            get => TekstFromTextBox("//input[@id='Type']");
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
            WaitOnAddNew();
        }
    }
}
