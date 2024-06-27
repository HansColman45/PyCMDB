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
            editPage.WebDriver = Driver;
            editPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_create");
            AssetType assetType = new()
            {
                Category = category,
                Vendor = vendor + rndNr.ToString(),
                Type = type + rndNr.ToString()
            };
            editPage.Category = assetType.Category;
            editPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Category");
            editPage.Vendor = assetType.Vendor;
            editPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Vendor");
            editPage.Type = assetType.Type;
            editPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Type");
            editPage.Create();
            editPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_created");
            return assetType;
        }
        public void SearchAssetType(AssetType assetType)
        {
            var page = GetAbility<MainPage>();
            page.Search(assetType.Type);
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_searched");
            ExpectedLog = $"The {assetType.Category} type Vendor: {assetType.Vendor} and type {assetType.Type} is created by {admin.Account.UserID} in table assettype";
        }
    }
}
