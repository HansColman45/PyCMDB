using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMDB.UI.Tests.Pages
{
    public class MainPage : Page
    {
        public MainPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        protected string NewXpath => "//a[.=' Add']";
        protected string EditXpath => "//a[@title='Edit']";
        protected string DeactivateXpath => "//a[@title='Deactivate']";
        protected string InfoXpath => "//a[@title='Info']";
        protected string ActivateXpath => "//a[@title='Activate']";

        public bool LoggedIn()
        {
            return IsElementVisable(By.XPath("//h1"));
        }
        public IdentityOverviewPage IdentityOverview()
        {
            ClickElementByXpath("//a[@id='Identity']");
            ClickElementByXpath("//a[@id='Identity2']");
            ClickElementByXpath("//a[@href='/Identity']");
            WaitOnAddNew();
            return new(driver);
        }
        public AssetTypeOverviewPage AssetTypeOverview()
        {
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Asset Type28']");
            ClickElementByXpath("//a[@href='/AssetType']");
            WaitOnAddNew();
            return new(driver);
        }
        public void Create()
        {
            ClickElementByXpath("//button[.='Create']");
        }
        public void Search(string searchstring)
        {
            EnterInTextboxByXPath("//input[@name='search']", searchstring);
            ClickElementByXpath("//button[@type='submit']");
        }
        protected void WaitOnAddNew()
        {
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
