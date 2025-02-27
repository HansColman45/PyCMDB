namespace CMDB.UI.Specflow.Abilities.Pages.Kensington
{
    public class KensingtonEditPage: MainPage
    {
        public string SerialNumber
        {
            set => EnterInTextboxByXPath("//input[@id='SerialNumber']", value);
        }
        public int AmountOfKeys
        {
            set => EnterInTextboxByXPath("//input[@id='AmountOfKeys']", value.ToString());
        }
        public bool HasLock
        {
            set => ClickElementByXpath($"//input[@id='HasLock' and @value='{value}']");
        }
        public string Type
        {
            set => SelectValueInDropDownByXpath("//select[@id='Type']", value);
        }
        public void Edit()
        {
            ClickElementByXpath("//button[.='Edit']");
        }
    }
}
