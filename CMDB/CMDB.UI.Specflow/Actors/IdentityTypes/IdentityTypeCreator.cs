using CMDB.UI.Specflow.Abilities.Pages.Types;
using CMDB.UI.Specflow.Questions.Types;
using CMDB.UI.Specflow.Tasks;

namespace CMDB.UI.Specflow.Actors.IdentityTypes
{
    public class IdentityTypeCreator : IdentityTypeActor
    {
        public IdentityTypeCreator(ScenarioContext scenarioContext, string name = "IdentityTypeCreator") : base(scenarioContext, name)
        {
        }
        public void CreateIdentityType(Helpers.IdentiyType identiyType)
        {
            rndNr = rnd.Next();
            var page = Perform(new OpenTheTypeCreatePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            page.Type = identiyType.Type + rndNr.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_type");
            page.Description = identiyType.Description + rndNr.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_description");
            page.Create();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_created");
        }
        public void SearchIdentityType(Helpers.IdentiyType identiyType)
        {
            var page = GetAbility<TypeOverviewPage>();
            page.Search(identiyType.Type + rndNr.ToString());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Identitytype with type: {identiyType.Type + rndNr.ToString()} and description: {identiyType.Description + rndNr.ToString()}",
                admin.Account.UserID, Table);
        }
    }
}
