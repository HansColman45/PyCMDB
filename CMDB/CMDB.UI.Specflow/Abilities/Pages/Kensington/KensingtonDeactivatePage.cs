using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Kensington
{
    public class KensingtonDeactivatePage : MainPage
    {
        public KensingtonDeactivatePage(IWebDriver web) : base(web)
        {
        }
        public string Reason
        {
            set => EnterInTextboxByXPath("//input[@id='reason']", value);
        }
        public void Delete()
        {
            ClickElementByXpath("//button[.='Delete']");
        }
    }
}
