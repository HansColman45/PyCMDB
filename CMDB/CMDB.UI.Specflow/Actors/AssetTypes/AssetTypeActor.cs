using CMDB.UI.Specflow.Questions.AssetType;

namespace CMDB.UI.Specflow.Actors.AssetTypes
{
    public class AssetTypeActor : CMDBActor
    {
        protected string Table => "assettype";
        public AssetTypeActor(ScenarioContext scenarioContext, string name = "AssetTypeActor") : base(scenarioContext, name)
        {
        }
        public string AssetTypeLastLogLine
        {
            get
            {
                var page = Perform(new OpenTheAssetTypeDetailPage());
                page.WebDriver = Driver;
                page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
                return page.GetLastLog();
            }
        }
    }
}
