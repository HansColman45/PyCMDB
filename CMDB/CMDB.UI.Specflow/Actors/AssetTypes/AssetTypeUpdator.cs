using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;
using CMDB.UI.Specflow.Actors.AccountTypes;
using CMDB.UI.Specflow.Questions.AssetType;
using CMDB.UI.Specflow.Tasks;

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
        public AssetType UpdateAssetType(AssetType assetType)
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
            ExpectedLog = GenericLogLineCreator.UpdateLogLine("Type", oldtype, assetType.Type, admin.Account.UserID, Table);
            return assetType;
        }
        public void DeActivateAssetType(AssetType assetType, string reason)
        {
            var deletePage = Perform(new OpenTheAssetTypeDeactivatePage());
            deletePage.WebDriver = Driver;
            deletePage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_deletePage");
            deletePage.Reason = reason;
            deletePage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Reason");
            deletePage.Delete();
            deletePage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_deleted");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Assettype with Vendor: {assetType.Vendor} and type: {assetType.Type}", admin.Account.UserID, reason, Table);
        }
        public void ActivateAssetType(AssetType assetType)
        {
            var page = GetAbility<AssetTypeOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_activated");
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Assettype with Vendor: {assetType.Vendor} and type: {assetType.Type}", admin.Account.UserID, Table);
        }
    }
}
