using CMDB.UI.Specflow.Abilities.Pages.Laptop;
using CMDB.UI.Specflow.Questions.Laptop;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.Laptops
{
    public class LaptopCreator : LaptopActor
    {
        public LaptopCreator(ScenarioContext scenarioContext, string name = "LaptopCreator") : base(scenarioContext, name)
        {
        }
        public void CreateNewLaptop(Helpers.Laptop laptop)
        {
            rndNr = rnd.Next();
            var page = Perform(new OpenTheLaptopCreatePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            page.AssetTag = laptop.AssetTag + rndNr.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_assetTag");
            page.SerialNumber = laptop.SerialNumber + rndNr.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_serialNumber");
            page.RAM = laptop.RAM;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ram");
            string Vendor, Type;
            Vendor = laptop.Type.Split(" ")[0];
            Type = laptop.Type.Split(" ")[1];
            var assetType = GetOrCreateAssetType("Laptop",Vendor, Type).Result;
            page.Type = assetType.TypeID.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_type");
            page.Create();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_created");
            log.Info($"Laptop with AssetTag {laptop.AssetTag + rndNr.ToString()} and type {laptop.Type} created");
        }
        public void SearchLaptop(Helpers.Laptop laptop)
        {
            var page = GetAbility<LaptopOverviewPage>();
            page.Search(laptop.AssetTag + rndNr.ToString());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Laptop with type {laptop.Type}",admin.Account.UserID, Table);
        }
    }
}
