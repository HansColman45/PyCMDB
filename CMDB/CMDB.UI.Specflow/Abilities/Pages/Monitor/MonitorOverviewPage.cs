namespace CMDB.UI.Specflow.Abilities.Pages.Monitor
{
    public class MonitorOverviewPage : MainPage
    {
        public MonitorOverviewPage() : base()
        {
        }
        public void Activate()
        {
            ClickElementByXpath(ActivateXpath);
            WaitUntilElmentVisableByXpath(NewXpath);
        }
    }
}
