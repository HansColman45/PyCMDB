using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Kensington
{
    class KensingtonCreatePage: MainPage
    {
        public KensingtonCreatePage(IWebDriver web) : base(web)
        {
        }
        public string SerialNumber
        {
            set => EnterInTextboxByXPath("//input[@id='SerialNumber']", value);
        }
        public int AmountOfKeys
        {
            set => EnterInTextboxByXPath("//input[@id='AmountOfKeys']", value.ToString());
        }
        public bool HasLock
        {
            set => ClickElementByXpath($"//input[@id='HasLock' and @value='{value}']");
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
