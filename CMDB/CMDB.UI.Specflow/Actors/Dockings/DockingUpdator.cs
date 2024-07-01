
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.Docking;
using CMDB.UI.Specflow.Tasks;
using Microsoft.Graph;

namespace CMDB.UI.Specflow.Actors.Dockings
{
    public class DockingUpdator : DockingActor
    {
        public DockingUpdator(ScenarioContext scenarioContext, string name = "DockingUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Docking> CreateNewDocking(bool active = false)
        {
            var context = GetAbility<DataContext>();
            return await context.CreateDocking(admin, active);
        }
        public async Task<Docking> UpdateDocking(Docking docking, string field, string value)
        {
            rndNr = rnd.Next();
            var updatePage = Perform(new OpenTheDockingEditPage());
            updatePage.WebDriver = Driver;
            updatePage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_EditPage");
            switch (field)
            {
                case "SerialNumber":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, docking.SerialNumber, value + rndNr, admin.Account.UserID, Table);
                    updatePage.SerialNumber = value + rndNr;
                    docking.SerialNumber = value + rndNr;
                    updatePage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_SerialNumber");
                    
                    break;
                case "Type":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, $"{docking.Type}", value, admin.Account.UserID, Table);
                    string Vendor, Type;
                    Vendor = value.Split(" ")[0];
                    Type = value.Split(" ")[1];
                    var assetType = await GetOrCreateDockingAssetType(Vendor, Type);
                    updatePage.Type = assetType.TypeID.ToString();
                    docking.Type = assetType;
                    updatePage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Type");
                    break;
                default: 
                    log.Fatal($"The update for the field {field} is not implemented");
                    throw new NotImplementedException();
            }
            updatePage.Edit();
            updatePage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_EditPage");
            return docking;
        }
    }
}
