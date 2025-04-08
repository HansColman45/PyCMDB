namespace CMDB.UI.Specflow.Abilities.Pages.Laptop
{
    public class LaptopAssignKensingtonPage : MainPage
    {
        public void SelectKensington(Domain.Entities.Kensington key)
        {
            SelectValueInDropDownByXpath("//select[@id='Kensington']", $"{key.KeyID}");
        }
    }
}
