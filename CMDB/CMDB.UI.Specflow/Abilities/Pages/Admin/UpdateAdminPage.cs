using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Admin
{
    public class UpdateAdminPage: MainPage
    {
        public UpdateAdminPage(IWebDriver driver): base(driver)
        {
        }
        public string Level
        {
            set => SelectValueInDropDownByXpath("//select[@id='Level']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
            WaitOnAddNew();
        }
    }
}
