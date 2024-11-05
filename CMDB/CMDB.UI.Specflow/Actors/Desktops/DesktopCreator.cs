using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Questions.Desktop;

namespace CMDB.UI.Specflow.Actors.Desktops
{
    public class DesktopCreator : DesktopActor
    {
        public DesktopCreator(ScenarioContext scenarioContext, string name = "DesktopCreator") : base(scenarioContext, name)
        {
        }
        public async Task<Helpers.Desktop> CreateNewDesktop(Helpers.Desktop desktop)
        {
            string Vendor, Type;
            Vendor = desktop.Type.Split(" ")[0];
            Type = desktop.Type.Split(" ")[1];
            var assetType = await GetOrCreateAssetType("Desktop", Vendor, Type);
            rndNr = rnd.Next();
            var createPage = Perform(new OpenTheDesktopCreatePage());
            createPage.WebDriver = Driver;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
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
