using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Monitor;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Monitor;

namespace CMDB.UI.Specflow.Actors.Monitors
{
    public class MonitorUpdator : MonitorActor
    {
        public MonitorUpdator(ScenarioContext scenarioContext, string name = "MonitorActor") : base(scenarioContext, name)
        {
        }
        public async Task<Domain.Entities.Screen> CreateNewMonitor(bool active = true)
        {
            if (active)
                return await Perform(new CreateTheMonitor());
            else
                return await Perform(new CreateTheInactiveMonitor());
        }
        public async Task<Domain.Entities.Screen> UpdateMonitor(Domain.Entities.Screen screen, string field, string value)
        {
            rndNr = rnd.Next();
            switch(field)
            {
                case "SerialNumber":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field,screen.SerialNumber, value+rndNr,admin.Account.UserID,Table);
                    var page = Perform(new OpenTheMonitorEditPage());
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
                    page.SerialNumber = value + rndNr;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
                    screen.SerialNumber = value + rndNr;
                    page.Edit();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Edited");
                    break;
                case "Type":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field,$"{screen.Type}", value,admin.Account.UserID,Table);
                    string Vendor, Type;
                    Vendor = value.Split(" ")[0];
                    Type = value.Split(" ")[1];
                    var assetType = await GetOrCreateAssetType("Monitor", Vendor, Type);
                    page = Perform(new OpenTheMonitorEditPage());
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
                    page.Type = assetType.TypeID.ToString();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
                    screen.Type = assetType;
                    page.Edit();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Edited");
                    break;
                default:
                    log.Error($"The update for Field {field} not supported");
                    throw new Exception($"The update for Field {field} not supported");
            }
            return screen;
        }
        public void DeactivateMonitor(Domain.Entities.Screen screen, string reason)
        {
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Monitor with type {screen.Type}", admin.Account.UserID, reason, Table);
            var page = Perform(new OpenTheMonitorDeactivatePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeletePage");
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            page.Delete();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        public void ActivateMonitor(Domain.Entities.Screen screen)
        {
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Monitor with type {screen.Type}", admin.Account.UserID, Table);
            var page = GetAbility<MonitorOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
    }
}
