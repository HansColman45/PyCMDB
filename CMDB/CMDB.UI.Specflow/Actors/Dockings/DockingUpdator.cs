using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Pages.Docking;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Docking;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.Dockings
{
    public class DockingUpdator : DockingActor
    {
        public DockingUpdator(ScenarioContext scenarioContext, string name = "DockingUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Docking> CreateNewDocking(bool active = true)
        {
            if(active)
                return await Perform(new CreateTheDockingStation());
            else
                return await Perform(new CreateTheIncativeDockingStation());
        }
        public async Task<Docking> UpdateDocking(Docking docking, string field, string value)
        {
            rndNr = rnd.Next();
            var updatePage = Perform(new OpenTheDockingEditPage());
            updatePage.WebDriver = Driver;
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
            switch (field)
            {
                case "SerialNumber":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, docking.SerialNumber, value + rndNr, admin.Account.UserID, Table);
                    updatePage.SerialNumber = value + rndNr;
                    docking.SerialNumber = value + rndNr;
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
                    
                    break;
                case "Type":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, $"{docking.Type}", value, admin.Account.UserID, Table);
                    string Vendor, Type;
                    Vendor = value.Split(" ")[0];
                    Type = value.Split(" ")[1];
                    var assetType = await GetOrCreateDockingAssetType(Vendor, Type);
                    updatePage.Type = assetType.TypeID.ToString();
                    docking.Type = assetType;
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
                    break;
                default: 
                    log.Fatal($"The update for the field {field} is not implemented");
                    throw new NotImplementedException();
            }
            updatePage.Edit();
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
            return docking;
        }
        public void DeactivateDocking(Docking docking, string reason)
        {
            var page = Perform(new OpenTheDockingDeactivatePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            page.Delete();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deleted");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Docking station with type {docking.Type}", admin.Account.UserID, reason,Table);
        }
        public void ActivateDocking(Docking docking)
        {
            var page = GetAbility<DockingOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ACtivated");
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Docking station with type {docking.Type}", admin.Account.UserID, Table);
        }
    }
}
