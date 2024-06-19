namespace CMDB.UI.Specflow.Abilities.Pages.AccountPages
{
    public class AccountDetailPage : MainPage
    {
        public AccountDetailPage() : base()
        {
        }
        public int Id
        {
            get
            {
                string id = GetAttributeFromXpath("//td[@id='Id']", "innerHTML");
                return Int32.Parse(id);
            }
        }
    }
}
