

using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.Laptop;

namespace CMDB.UI.Specflow.Actors.Laptops
{
    public class LaptopActor : CMDBActor
    {
        protected string Table => "laptop";
        public LaptopActor(ScenarioContext scenarioContext, string name = "LaptopActor") : base(scenarioContext, name)
        {
        }
        public string LaptopLastLogLine
        {
            get
            {
                var page = Perform(new OpenTheLaptopDetailPage());
                page.WebDriver = Driver;
                return page.GetLastLog();
            }
        }
        protected async Task<Domain.Entities.AssetType> GetOrCreateLaptopType(string vendor, string type)
        {
            var context = GetAbility<DataContext>();
            var assetCat = context.GetAssetCategory("Laptop");
            return await context.GetOrCreateAssetType(vendor, type, assetCat);
        }
    }
}
