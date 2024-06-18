using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Mobile
{
    public class UpdateMobilePage : MainPage
    {
        public UpdateMobilePage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public string IMEI
        {
            set => EnterInTextboxByXPath("//input[@id='IMEI']", value);
            get => TekstFromElementByXpath("//input[@id='IMEI']");
        }
        public string Type
        {
            set => SelectValueInDropDownByXpath("//select[@id='MobileType.Id']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
            WaitOnAddNew();
        }
    }
}
