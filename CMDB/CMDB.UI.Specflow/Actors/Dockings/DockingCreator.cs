using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Questions.Docking;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.Dockings
{
    public class DockingCreator : DockingActor
    {
        public DockingCreator(ScenarioContext scenarioContext, string name = "DockingCreator") : base(scenarioContext, name)
        {
        }
        public async Task<Helpers.DockingStation> CreateNewDocking(Helpers.DockingStation dockingStation)
        {
            rndNr = rnd.Next();
            var createPage = Perform(new OpenTheDockingCreatePage());
            createPage.WebDriver = Driver;
            createPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_CreatePage");
            createPage.SerialNumber = dockingStation.SerialNumber + rndNr;
            dockingStation.SerialNumber += rndNr;
            createPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_SerialNumber");
            createPage.AssetTag = dockingStation.AssetTag + rndNr;
            dockingStation.AssetTag += rndNr;
            string Vendor, Type;
            Vendor = dockingStation.Type.Split(" ")[0];
            Type = dockingStation.Type.Split(" ")[1];
            var assetType = await GetOrCreateDockingAssetType(Vendor, Type);
            createPage.Type = assetType.TypeID.ToString();
            createPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Type");
            createPage.Create();
            return dockingStation;
        }
        public void SearchDocking(Helpers.DockingStation dockingStation)
        {
            var page = GetAbility<MainPage>();
            page.Search(dockingStation.AssetTag);
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Docking station with type {dockingStation.Type}", admin.Account.UserID, Table);
        }
    }
}
