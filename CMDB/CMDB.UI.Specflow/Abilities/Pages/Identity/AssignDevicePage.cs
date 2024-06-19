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
        public AssignFormPage Assign()
        {
            ClickElementByXpath("//button[@type='submit']");
            return new();
        }
    }
}
