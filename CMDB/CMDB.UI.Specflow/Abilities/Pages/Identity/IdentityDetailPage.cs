using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class IdentityDetailPage : MainPage
    {
        public IdentityDetailPage() : base()
        {
        }
        public int Id
        {
            get
            {
                string id = GetAttributeFromXpath("//td[@id='Id']", "innerHTML");
                return int.Parse(id);
            }
        }
        public ReleaseAccountPage ReleaseAccount()
        {
            ClickElementByXpath(ReleaseAccountXPath);
            return new();
        }
        public ReleaseDevicePage ReleaseDevice()
        {
            ClickElementByXpath(ReleaseDeviceXPath);
            return new();
        }
    }
}
