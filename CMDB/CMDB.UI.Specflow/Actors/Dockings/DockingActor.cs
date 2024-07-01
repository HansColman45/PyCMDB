using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.Docking;

namespace CMDB.UI.Specflow.Actors.Dockings
{
    public class DockingActor : CMDBActor
    {
        protected string Table => "docking";
        public DockingActor(ScenarioContext scenarioContext, string name = "DockingActor") : base(scenarioContext, name)
        {
        }
        public string DockingLastLogLine
        {
            get
            {
                var overviewPAge = Perform(new OpenTheDockingDetailPage());
                overviewPAge.WebDriver = Driver;
                overviewPAge.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
                return overviewPAge.GetLastLog();
            }
        }
        protected async Task<Domain.Entities.AssetType> GetOrCreateDockingAssetType(string vendor, string type)
        {
            var context = GetAbility<DataContext>();
            var assetCat = context.GetAssetCategory("Docking station");
            return await context.GetOrCreateAssetType(vendor, type, assetCat);
        }
    }
}
