using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Questions.Docking;

namespace CMDB.UI.Specflow.Actors.Dockings
{
    public class DockingCreator : DockingActor
    {
        public DockingCreator(ScenarioContext scenarioContext, string name = "DockingCreator") : base(scenarioContext, name)
        {
        }
        public async Task<Helpers.DockingStation> CreateNewDocking(Helpers.DockingStation dockingStation)
        {
            string Vendor, Type;
            Vendor = dockingStation.Type.Split(" ")[0];
            Type = dockingStation.Type.Split(" ")[1];
            var assetType = await GetOrCreateAssetType("Docking station", Vendor, Type);
            rndNr = rnd.Next();
            var createPage = Perform(new OpenTheDockingCreatePage());
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            createPage.SerialNumber = dockingStation.SerialNumber + rndNr;
            dockingStation.SerialNumber += rndNr;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
            createPage.AssetTag = dockingStation.AssetTag + rndNr;
            dockingStation.AssetTag += rndNr;
            createPage.Type = assetType.TypeID.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
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
