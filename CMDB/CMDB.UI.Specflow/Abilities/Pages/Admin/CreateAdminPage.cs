using CMDB.Domain.Entities;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Admin
{
    public class CreateAdminPage: MainPage
    {
        public CreateAdminPage(IWebDriver driver): base(driver)
        {
        }
        public void SelectAccount(Account account)
        {
            SelectValueInDropDownByXpath("//select[@id='Account']", $"{account.AccID}");
        }
        public string Level 
        { 
            set => SelectValueInDropDownByXpath("//select[@id='Level']", value); 
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
