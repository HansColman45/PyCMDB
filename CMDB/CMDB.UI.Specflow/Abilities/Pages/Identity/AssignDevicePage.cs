using CMDB.Domain.Entities;

namespace CMDB.UI.Specflow.Abilities.Pages.Identity
{
    public class AssignDevicePage : MainPage
    {
        public AssignDevicePage() : base()
        {
        }
        public void ClickDevice(Device device)
        {
            ClickElementByXpath($"//*[@id='{device.AssetTag}']");
        }
        public void ClickMobile(Domain.Entities.Mobile mobile)
        {
            ClickElementByXpath($"//*[@id='{mobile.IMEI}']");
        }
        public AssignFormPage Assign()
        {
            ClickElementByXpath("//button[@type='submit']");
            return new();
        }
    }
}
