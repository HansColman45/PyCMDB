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
        public string Title => GetAttributeFromXpath("//h2", "innerHTML");
        public bool IsVaidationErrorVisable()
        {
            return IsElementVisable(By.XPath("//div[@class='text-danger validation-summary-errors']"));
        }
        protected static string NewXpath => "//a[.=' Add']";
        protected static string EditXpath => "//a[@title='Edit']";
        protected static string DeactivateXpath => "//a[@title='Deactivate']";
        protected static string InfoXpath => "//a[@title='Info']";
        protected static string ActivateXpath => "//a[@title='Activate']";
        protected static string AssignIdenityXpath => "//a[@title='Assign Identity']";
        protected static string ReleaseIdenityXpath => "//a[@title='Release Identity']";
        public static string ReleaseDeviceXPath => "//a[@id='ReleaseDevice']";
        public static string ReleaseAccountXPath => "//a[@id='ReleaseAccount']";

        public bool LoggedIn()
        {
            return IsElementVisable(By.XPath("//h1"));
        }
        public IdentityOverviewPage IdentityOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Identity']");
            ClickElementByXpath("//a[@id='Identity']");
            ClickElementByXpath("//a[@id='Identity2']");
            ClickElementByXpath("//a[@href='/Identity']");
            WaitOnAddNew();
            return new(driver);
        }
        public AccountOverviewPage AccountOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Account']");
            ClickElementByXpath("//a[@id='Account']");
            ClickElementByXpath("//a[@id='Account5']");
            ClickElementByXpath("//a[@href='/Account']");
            WaitOnAddNew();
            return new(driver);
        }
        public void RoleOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Role']");
            ClickElementByXpath("//a[@id='Role']");
            ClickElementByXpath("//a[@id='Role8']");
            ClickElementByXpath("//a[@href='/Role']");
            WaitOnAddNew();
            //return new(driver);
        }
        public LaptopOverviewPage LaptopOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Laptop11']");
            ClickElementByXpath("//a[@href='/Laptop']");
            WaitOnAddNew();
            return new(driver);
        }
        public DesktopOverviewPage DesktopOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Desktop13']");
            ClickElementByXpath("//a[@href='/Desktop']");
            WaitOnAddNew();
            return new(driver);
        }
        public MonitorOverviewPage MonitorOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Monitor15']");
            ClickElementByXpath("//a[@href='/Monitor']");
            WaitOnAddNew();
            return new(driver);
        }
        public DockingOverviewPage DockingStationOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Docking station17']");
            ClickElementByXpath("//a[@href='/Docking']");
            WaitOnAddNew();
            return new(driver);
        }
        public TokenOverviewPage TokenOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Token19']");
            ClickElementByXpath("//a[@href='/Token']");
            WaitOnAddNew();
            return new(driver);
        }
        public void KensingtonOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Kensington21']");
            ClickElementByXpath("//a[@href='/Kensington']");
            WaitOnAddNew();
            //return new(driver);
        }
        public MobileOverviewPage MobileOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Mobile23']");
            ClickElementByXpath("//a[@href='/Mobile']");
            WaitOnAddNew();
            return new(driver);
        }
        public void SubscriptionOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Devices']");
            ClickElementByXpath("//a[@id='Subscription25']");
            ClickElementByXpath("//a[@href='/Subscription']");
            WaitOnAddNew();
            //return new(driver);
        }
        public AssetTypeOverviewPage AssetTypeOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Asset Type28']");
            ClickElementByXpath("//a[@href='/AssetType']");
            WaitOnAddNew();
            return new(driver);
        }
        public void AssetCategoryOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Asset Category30']");
            ClickElementByXpath("//a[@href='/AssetCategory']");
            WaitOnAddNew();
            //return new(driver);
        }
        public TypeOverviewPage IdentityTypeOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Identity Type32']");
            ClickElementByXpath("//a[@href='/IdentityType']");
            WaitOnAddNew();
            return new(driver);
        }
        public TypeOverviewPage AccountTypeyOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Account Type34']");
            ClickElementByXpath("//a[@href='/AccountType']");
            WaitOnAddNew();
            return new(driver);
        }
        public TypeOverviewPage RoleTypeOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Types']");
            ClickElementByXpath("//a[@id='Role Type36']");
            ClickElementByXpath("//a[@href='/RoleType']");
            WaitOnAddNew();
            return new(driver);
        }
        public void SubscriptionTypeOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Subscription Type38']");
            ClickElementByXpath("//a[@id='Subscription Type38']");
            ClickElementByXpath("//a[@id='Subscription Type38']");
            ClickElementByXpath("//a[@href='/SubscriptionType']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void SystemOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='System']");
            ClickElementByXpath("//a[@id='System']");
            ClickElementByXpath("//a[@id='System41']");
            ClickElementByXpath("//a[@href='/System']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void ApplicationOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Application']");
            ClickElementByXpath("//a[@id='Application']");
            ClickElementByXpath("//a[@id='Application44']");
            ClickElementByXpath("//a[@href='/Application']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void AdminOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Admin']");
            ClickElementByXpath("//a[@id='Admin']");
            ClickElementByXpath("//a[@id='Admin47']");
            ClickElementByXpath("//a[@href='/Admin']");
            WaitOnAddNew();
            //return new(driver);
        }
        public void PermissionOverview()
        {
            WaitUntilElmentVisableByXpath("//a[@id='Admin']");
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
