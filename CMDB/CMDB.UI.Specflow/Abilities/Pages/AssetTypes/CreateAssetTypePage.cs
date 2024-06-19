using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.AssetTypes
{
    public class CreateAssetTypePage : MainPage
    {
        public CreateAssetTypePage() : base()
        {
        }
        public string Category
        {
            set => SelectTektInDropDownByXpath("//select[@id='Category']", value);
        }
        public string Vendor
        {
            set => EnterInTextboxByXPath("//input[@id='Vendor']", value);
        }
        public string Type
        {
            set => EnterInTextboxByXPath("//input[@id='Type']", value);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
