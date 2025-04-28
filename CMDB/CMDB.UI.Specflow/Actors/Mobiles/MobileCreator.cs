using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Mobile;
using CMDB.UI.Specflow.Questions.Mobile;

namespace CMDB.UI.Specflow.Actors.Mobiles
{
    public class MobileCreator : MobileActor
    {
        public MobileCreator(ScenarioContext scenarioContext, string name = "MobileCreator") : base(scenarioContext, name)
        {
        }
        public async Task CreateNewMobile(Helpers.Mobile mobile)
        {
            rndNr = rnd.Next();
            string Vendor, Type;
            Vendor = mobile.Type.Split(" ")[0];
            Type = mobile.Type.Split(" ")[1];
            var assetType = await GetOrCreateAssetType("Mobile", Vendor, Type);
            var page = Perform(new OpenTheMobileCreatePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            page.IMEI = mobile.IMEI + rndNr.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_IMEI");
            page.Type = assetType.TypeID.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_type");
            page.Create();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_created");
        }
        public void SearchMobile(Helpers.Mobile mobile)
        {
            var page = GetAbility<MobileOverviewPage>();
            page.Search(mobile.IMEI + rndNr.ToString());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Mobile with type {mobile.Type}",admin.Account.UserID, Table);
        }
    }
}
