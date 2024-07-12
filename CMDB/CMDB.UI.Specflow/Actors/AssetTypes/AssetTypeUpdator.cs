using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Pages.AssetTypes;
using CMDB.UI.Specflow.Questions.AssetType;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
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
            if (active)
                return await Perform(new CreateTheAssetType());
            else
                return await Perform(new CreateTheInactiveAssetType());
        }
        public AssetType UpdateAssetType(AssetType assetType)
        {
            var oldtype = assetType.Type;
            rndNr = rnd.Next();
            var editPage = Perform(new OpenTheAssetTypeEditPage());
            editPage.WebDriver = Driver;
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_editPage");
            assetType.Type = "Orange" + rndNr.ToString();
            editPage.Type = assetType.Type;
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            editPage.Edit();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_edited");
            ExpectedLog = GenericLogLineCreator.UpdateLogLine("Type", oldtype, assetType.Type, admin.Account.UserID, Table);
            return assetType;
        }
        public void DeActivateAssetType(AssetType assetType, string reason)
        {
            var deletePage = Perform(new OpenTheAssetTypeDeactivatePage());
            deletePage.WebDriver = Driver;
            deletePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deletePage");
            deletePage.Reason = reason;
            deletePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            deletePage.Delete();
            deletePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deleted");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"{assetType.Category.Category} type Vendor: {assetType.Vendor} and type {assetType.Type}", admin.Account.UserID, reason, Table);
        }
        public void ActivateAssetType(AssetType assetType)
        {
            var page = GetAbility<AssetTypeOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_activated");
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"{assetType.Category.Category} type Vendor: {assetType.Vendor} and type {assetType.Type}", admin.Account.UserID, Table);
        }
    }
}
