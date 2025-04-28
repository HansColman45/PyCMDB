using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Types
{
    public class UpdateTypePage : MainPage
    {
        public UpdateTypePage(IWebDriver web) : base(web)
        {
        }
        public string Type
        {
            set => EnterInTextboxByXPath("//input[@id='Type']", value);
        }
        public string Description
        {
            set => EnterInTextboxByXPath("//input[@id='Description']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}
