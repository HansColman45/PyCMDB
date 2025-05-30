using CMDB.UI.Specflow.Questions.AssetType;
using Reqnroll;

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
                page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_detail");
                return page.GetLastLog();
            }
        }
    }
}
