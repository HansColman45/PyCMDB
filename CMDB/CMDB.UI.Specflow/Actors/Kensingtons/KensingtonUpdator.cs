
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Keys;

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
            var page = Perform(new OpenTheKensingtonEditPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
            switch (field)
            {
                case "SerialNumber":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, kensington.SerialNumber, newValue, admin.Account.UserID, Table);
                    page.SerialNumber = newValue;
                    kensington.SerialNumber = newValue;
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
    }
}
