using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Types
{
    public class CreateTypePage : MainPage
    {
        public CreateTypePage() : base()
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
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
    }
}
