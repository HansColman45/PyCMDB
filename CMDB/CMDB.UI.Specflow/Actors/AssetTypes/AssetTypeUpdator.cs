using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Actors.AccountTypes;
using CMDB.UI.Specflow.Questions.AssetType;

namespace CMDB.UI.Specflow.Actors.AssetTypes
{
    public class AssetTypeUpdator : AssetTypeActor
    {
        public AssetTypeUpdator(ScenarioContext scenarioContext, string name = "AccountTypeUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<AssetType> CreateAssetType(bool active = true)
        {
            var context = GetAbility<DataContext>();
            return await context.CreateAssetType(admin,active);
        }
        public AssetType UpdateAccountType(AssetType assetType)
        {
            var oldtype = assetType.Type;
            rndNr = rnd.Next();
            var editPage = Perform(new OpenTheAssetTypeEditPage());
            editPage.WebDriver = Driver;
            editPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_editPage");
            assetType.Type = "Orange" + rndNr.ToString();
            editPage.Type = assetType.Type;
            editPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Type");
            editPage.Edit();
            editPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_edited");
            ExpectedLog = $"The Type has been changed from {oldtype} to {assetType.Type} by {admin.Account.UserID} in table assettype";
            return assetType;
        }
    }
}
