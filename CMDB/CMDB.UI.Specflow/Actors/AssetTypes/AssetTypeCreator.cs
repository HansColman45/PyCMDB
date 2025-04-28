using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Helpers;
using CMDB.UI.Specflow.Questions.AssetType;

namespace CMDB.UI.Specflow.Actors.AssetTypes
{
    public class AssetTypeCreator : AssetTypeActor
    {
        public AssetTypeCreator(ScenarioContext scenarioContext) : base(scenarioContext, "AssetTypeCreator")
        {
        }

        public AssetType CreateAssetType(string category, string vendor, string type)
        {
            rndNr = rnd.Next();
            var editPage = Perform(new OpenTheAsseTypeCreatePage());
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            AssetType assetType = new()
            {
                Category = category,
                Vendor = vendor + rndNr.ToString(),
                Type = type + rndNr.ToString()
            };
            editPage.Category = assetType.Category;
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Category");
            editPage.Vendor = assetType.Vendor;
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Vendor");
            editPage.Type = assetType.Type;
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            editPage.Create();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_created");
            return assetType;
        }
        public void SearchAssetType(AssetType assetType)
        {
            var page = GetAbility<MainPage>();
            page.Search(assetType.Type);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_searched");
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"{assetType.Category} type Vendor: {assetType.Vendor} and type {assetType.Type}",admin.Account.UserID,Table);
        }
    }
}
