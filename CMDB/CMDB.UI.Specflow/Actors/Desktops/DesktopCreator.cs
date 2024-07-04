using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Questions.Desktop;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.Desktops
{
    public class DesktopCreator : DesktopActor
    {
        public DesktopCreator(ScenarioContext scenarioContext, string name = "DesktopCreator") : base(scenarioContext, name)
        {
        }
        private async Task<Domain.Entities.AssetType> GetOrCreateDesktopAssetType(string vendor, string type)
        {
            var context = GetAbility<DataContext>();
            var assetCat = context.GetAssetCategory("Desktop");
            return await context.GetOrCreateAssetType(vendor, type, assetCat);
        }
        public async Task<Helpers.Desktop> CreateNewDesktop(Helpers.Desktop desktop)
        {
            rndNr = rnd.Next();
            var createPage = Perform(new OpenTheDesktopCreatePage());
            createPage.WebDriver = Driver;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            string Vendor, Type;
            Vendor = desktop.Type.Split(" ")[0];
            Type = desktop.Type.Split(" ")[1];
            var assetType = await GetOrCreateDesktopAssetType(Vendor, Type);
            createPage.AssetTag = desktop.AssetTag + rndNr;
            desktop.AssetTag = desktop.AssetTag + rndNr;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_AssetTag");
            createPage.SerialNumber = desktop.SerialNumber + rndNr;
            desktop.SerialNumber = desktop.SerialNumber + rndNr;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
            createPage.RAM = desktop.RAM;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_RAM");
            createPage.Type = assetType.TypeID.ToString();
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            createPage.Create();
            return desktop;
        }
        public void SearchDesktop(Helpers.Desktop desktop)
        {
            var page = GetAbility<MainPage>();
            page.Search(desktop.AssetTag);
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Desktop with type {desktop.Type}", admin.Account.UserID,Table);
        }
    }
}
