
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Kensington;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Keys;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Kensingtons
{
    public class KensingtonUpdator : KensingtonActor
    {
        public KensingtonUpdator(ScenarioContext scenarioContext, string name = "KensingtonUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Kensington> CreateKensington(bool active = true)
        {
            try
            {
                if (active)
                    return await Perform(new CreateTheKensington());
                else
                    return await Perform(new CreateTheInactiveKensington());
            }
            catch (Exception e)
            {
                log.Fatal(e.Message);
                throw;
            }
        }
        public Kensington Update(Kensington kensington, string field, string newValue)
        {
            rndNr = rnd.Next();
            var page = Perform(new OpenTheKensingtonEditPage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
            switch (field)
            {
                case "SerialNumber":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, kensington.SerialNumber, $"{newValue}{rndNr}", admin.Account.UserID, Table);
                    page.SerialNumber = $"{newValue}{rndNr}";
                    kensington.SerialNumber = $"{newValue}{rndNr}";
                    break;
                case "AmountOfKeys":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, kensington.AmountOfKeys.ToString(), newValue, admin.Account.UserID, Table);
                    kensington.AmountOfKeys = Int32.Parse(newValue);
                    page.AmountOfKeys = Int32.Parse(newValue);
                    break;
                default:
                    log.Fatal($"The update for {field} is not implemented");
                    throw new Exception($"The update for {field} is not implemented");
            }
            page.Edit();
            return kensington;
        }
        public void DeactivateKensington(Kensington kensington, string reason)
        {
            var page = Perform(new OpenTheKensingtonDeactivatePage());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Kensington with serial number: {kensington.SerialNumber}", admin.Account.UserID, reason, Table);
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            page.Delete();
        }
        public void ActivateKensington(Kensington kensington)
        {
            var page = GetAbility<KensingtonOverviewPage>();
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Kensington with serial number: {kensington.SerialNumber}", admin.Account.UserID, Table);
            page.Activate();
        }
    }
}
