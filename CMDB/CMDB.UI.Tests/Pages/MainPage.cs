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
        public bool IsVaidationErrorVisable()
        {
            return IsElementVisable(By.XPath("//div[@class='text-danger validation-summary-errors']"));
        }
        protected static string NewXpath => "//a[.=' Add']";
        protected static string EditXpath => "//a[@title='Edit']";
        protected static string DeactivateXpath => "//a[@title='Deactivate']";
        protected static string InfoXpath => "//a[@title='Info']";
        protected static string ActivateXpath => "//a[@title='Activate']";

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
        public AccountOverviewPage AccountOverview()
        {
            ClickElementByXpath("//a[@id='Account']");
            ClickElementByXpath("//a[@id='Account5']");
            ClickElementByXpath("//a[@href='/Account']");
            WaitOnAddNew();
            return new(driver);
        }
        public void RoleOverview()
        {
            ClickElementByXpath("//a[@id='Role']");
            ClickElementByXpath("//a[@id='Role8']");
            ClickElementByXpath("//a[@href='/Role']");
            WaitOnAddNew();
            //return new(driver);
        }
        public LaptopOverviewPage LaptopOverview()
        {
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Laptop11']");
            ClickElementByXpath("//a[@href='/Laptop']");
            WaitOnAddNew();
            return new(driver);
        }
        public DesktopOverviewPage DesktopOverview()
        {
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Desktop13']");
            ClickElementByXpath("//a[@href='/Desktop']");
            WaitOnAddNew();
            return new(driver);
        }
        public void MonitorOverview()
        {
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Monitor15']");
            ClickElementByXpath("//a[@href='/Monitor']");
            WaitOnAddNew();
            //return new(driver);
        }
        public DockingOverviewPage DockingStationOverview()
        {
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Docking station17']");
            ClickElementByXpath("//a[@href='/Docking']");
            WaitOnAddNew();
            return new(driver);
        }
        public void TokenOverview()
        {
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Token19']");
            ClickElementByXpath("//a[@href='/Token']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void KensingtonOverview()
        {
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Kensington21']");
            ClickElementByXpath("//a[@href='/Kensington']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void MobileOverview()
        {
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Mobile23']");
            ClickElementByXpath("//a[@href='/Mobile']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void SubscriptionOverview()
        {
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Subscription25']");
            ClickElementByXpath("//a[@href='/Subscription']");
            WaitOnAddNew();
            //return new(driver);
        }
        public AssetTypeOverviewPage AssetTypeOverview()
        {
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Asset Type28']");
            ClickElementByXpath("//a[@href='/AssetType']");
            WaitOnAddNew();
            return new(driver);
        }
        public void AssetCategoryOverview()
        {
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Asset Category30']");
            ClickElementByXpath("//a[@href='/AssetCategory']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void IdentityTypeOverview()
        {
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Identity Type32']");
            ClickElementByXpath("//a[@href='/IdentityType']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void AccountTypeyOverview()
        {
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Account Type34']");
            ClickElementByXpath("//a[@href='/AccountType']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void RoleTypeOverview()
        {
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Role Type36']");
            ClickElementByXpath("//a[@href='/RoleType']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void SubscriptionTypeOverview()
        {
            ClickElementByXpath("//a[@id='Subscription Type38']");
            ClickElementByXpath("//a[@id='Subscription Type38']");
            ClickElementByXpath("//a[@href='/SubscriptionType']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void SystemOverview()
        {
            ClickElementByXpath("//a[@id='System']");
            ClickElementByXpath("//a[@id='System41']");
            ClickElementByXpath("//a[@href='/System']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void ApplicationOverview()
        {
            ClickElementByXpath("//a[@id='Application']");
            ClickElementByXpath("//a[@id='Application44']");
            ClickElementByXpath("//a[@href='/Application']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void AdminOverview()
        {
            ClickElementByXpath("//a[@id='Admin']");
            ClickElementByXpath("//a[@id='Admin47']");
            ClickElementByXpath("//a[@href='/Admin']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void PermissionOverview()
        {
            ClickElementByXpath("//a[@id='Admin']");
            ClickElementByXpath("//a[@id='Permissions49']");
            ClickElementByXpath("//a[@href='/Permission']");
            WaitOnAddNew();
            //return new(driver);
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
