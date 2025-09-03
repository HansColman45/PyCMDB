using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Permissions
{
    public class EditPermissionPage : MainPage
    {
        public EditPermissionPage(IWebDriver web) : base(web)
        {
        }
        public string Right
        {
            set
            {
                EnterInTextboxByXPath("//input[@id='Right']", value);
            }
        }
        public string Description
        {
            set
            {
                EnterInTextboxByXPath("//input[@id='Description']", value);
            }
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}
