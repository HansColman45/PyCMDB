using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Desktop;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Desktop;

namespace CMDB.UI.Specflow.Actors.Desktops
{
    public class DesktopUpdator : DesktopActor
    {
        public DesktopUpdator(ScenarioContext scenarioContext, string name = "DesktopUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Desktop> CreateDesktop(bool active = true)
        {
            if(active)
                return await Perform(new CreateTheDesktop());
            else
                return await Perform(new CreateTheInactiveDesktop());
        }
        public Desktop UpdateDesktop(Desktop desktop, string field, string value)
        {
            rndNr = rnd.Next();
            var updatePage = Perform(new OpenTheDesktopEditPage());
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_editPage");
            switch (field)
            {
                case "Serialnumber":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, desktop.SerialNumber, value + rndNr.ToString(), admin.Account.UserID, Table); 
                    desktop.SerialNumber = value + rndNr.ToString();
                    updatePage.SerialNumber = desktop.SerialNumber;
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
                    break;
                case "RAM":
                    var newRam = GetRam(value).Value;
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, desktop.RAM, $"{newRam}", admin.Account.UserID, Table);
                    desktop.RAM = value;
                    updatePage.RAM = desktop.RAM;
                    updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_RAM");
                    break;
                default:
                    log.Fatal($"The update for Field {field} is not implemented");
                    throw new NotImplementedException($"The update for Field {field} is not implemented");
            }
            updatePage.Edit();
            updatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Edited");
            return desktop;
        }
        public void DeactivateDesktop(Desktop desktop, string reason)
        {
            var deactivatePage = Perform(new OpenTheDesktopDeactivatePage());
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deactivatePage");
            deactivatePage.Reason = reason;
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            deactivatePage.Delete();
            deactivatePage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deleted");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Desktop with type {desktop.Type}", admin.Account.UserID, reason, Table);
        }
        public void ActivateDesktop(Desktop desktop)
        {
            var overviewPage = GetAbility<DesktopOverviewPage>();
            overviewPage.Activate();
            overviewPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Desktop with type {desktop.Type}", admin.Account.UserID, Table);
        }
    }
}