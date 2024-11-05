
using CMDB.UI.Specflow.Abilities.Pages.Monitor;
using CMDB.UI.Specflow.Questions.Monitor;
using CMDB.Infrastructure;

namespace CMDB.UI.Specflow.Actors.Monitors
{
    public class MonitorCreator : MonitorActor
    {
        public MonitorCreator(ScenarioContext scenarioContext, string name = "MonitorCreator") : base(scenarioContext, name)
        {
        }
        public async Task CreateNewMonitor(Helpers.Monitor monitor)
        {
            rndNr = rnd.Next();
            string Vendor, Type;
            Vendor = monitor.Type.Split(" ")[0];
            Type = monitor.Type.Split(" ")[1];
            var assetType = await GetOrCreateAssetType("Monitor", Vendor, Type);
            var page = Perform(new OpenTheCreateMonitorPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            page.AssetTag = monitor.AssetTag + rndNr;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssetTag");
            page.SerialNumber = monitor.SerialNumber + rndNr;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
            page.Type = assetType.TypeID.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            page.Create();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        public void SearchMonitor(Helpers.Monitor monitor)
        {
            var page = GetAbility<MonitorOverviewPage>();
            page.Search(monitor.AssetTag +rndNr);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Monitor with type {monitor.Type}", admin.Account.UserID, Table);
        }
    }
}
