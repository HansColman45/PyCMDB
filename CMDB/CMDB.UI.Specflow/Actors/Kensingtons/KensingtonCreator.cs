
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;
using CMDB.UI.Specflow.Helpers;
using CMDB.UI.Specflow.Questions.Keys;

namespace CMDB.UI.Specflow.Actors.Kensingtons
{
    class KensingtonCreator : KensingtonActor
    {
        public KensingtonCreator(ScenarioContext scenarioContext, string name = "KensingtonCreator") : base(scenarioContext, name)
        {
        }
        public async Task CreateKensington(Kensington kensington)
        {
            string Vendor, Type;
            Vendor = kensington.Type.Split(" ")[0];
            Type = kensington.Type.Split(" ")[1];
            var assetType = await GetOrCreateAssetType("Kensington", Vendor, Type);
            rndNr = rnd.Next();
            var page = Perform(new OpenTheKensingtonCreatePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            page.SerialNumber = $"{kensington.SerialNumber}{rndNr}";
            page.AmountOfKeys = kensington.AmountOfKeys;
            page.Type = assetType.TypeID.ToString();
            page.Create();
        }
        public void Search(Kensington kensington)
        {
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Kensington with serial number: {kensington.SerialNumber}{rndNr}",admin.Account.UserID,Table);
            var page = GetAbility<KensingtonOverviewPage>();
            page.Search($"{kensington.SerialNumber}{rndNr}");
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
        }
    }
}
