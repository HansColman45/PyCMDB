using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Pages.Laptop;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Laptop;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.Laptops
{
    public class LaptopUpdator : LaptopActor
    {
        public LaptopUpdator(ScenarioContext scenarioContext, string name = "LaptopActor") : base(scenarioContext, name)
        {
        }
        public async Task<Laptop> CreateLaptop(bool active = true)
        {
            if(active)
                return await Perform(new CreateTheLaptop());
            else
                return await Perform(new CreateTheInactiveLaptop());
        }
        public Laptop UpdateLaptop(Laptop laptop, string field, string value)
        {
            rndNr = rnd.Next();
            var page = Perform(new OpenTheLaptopEditPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_editPage");
            switch (field)
            {
                case "Serialnumber":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field,laptop.SerialNumber, value,admin.Account.UserID,Table);
                    page.SerialNumber = value + rndNr.ToString();
                    laptop.SerialNumber = value + rndNr.ToString();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_serialNumber");
                    break;
                case "RAM":
                    var newRam = GetRam(value).Value;
                    laptop.RAM = value;
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field,laptop.RAM, $"{newRam}",admin.Account.UserID,Table);
                    page.RAM = value;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_ram");
                    break;
                default:
                    log.Fatal($"The update for Field {field} is not implemented");
                    throw new NotImplementedException($"The update for Field {field} is not implemented");
            }
            page.Edit();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_edited");
            return laptop;
        }
        public void DeactivateLaptop(Laptop laptop, string reason)
        {
            var page = Perform(new OpenTheLaptopDeactivatePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deactivatePage");
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_reason");
            page.Delete();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_deleted");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Laptop with type {laptop.Type}",admin.Account.UserID,reason,Table);
        }
        public void ActivateLaptop(Laptop laptop)
        {
            var page = GetAbility<LaptopOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_activated");
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Laptop with type {laptop.Type}",admin.Account.UserID,Table);
        }
    }
}
