using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Questions.Identity;

namespace CMDB.UI.Specflow.Actors.IdentityActors
{
    public class IdentityCreator : IdentityActor
    {
        public IdentityCreator(ScenarioContext scenarioContext, string name = "IdentityCreator") : base(scenarioContext, name)
        {
        }
        public CreateIdentityPage OpenCreateIdentityPage()
        {
            var createPage = Perform(new OpenTheCreateIdentityPage());
            createPage.WebDriver = Driver;
            createPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_CreatePage");
            return createPage;
        }
        public void CreateNewIdentity(Helpers.Identity iden)
        {
            rndNr = rnd.Next();
            var page = GetAbility<CreateIdentityPage>();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_create");
            page.FirstName = iden.FirstName + rndNr.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_FirstName");
            page.LastName = iden.LastName + rndNr.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LastName");
            page.Email = iden.Email;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Email");
            page.Company = iden.Company;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Company");
            page.UserId = iden.UserId + rndNr.ToString();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UserId");
            page.Type = iden.Type;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
            page.Language = iden.Language;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Language");
        }
        public void SearchIdentity(Helpers.Identity iden)
        {
            var page = GetAbility<IdentityOverviewPage>();
            page.Search(iden.FirstName + rndNr.ToString());
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Identity width name: {iden.FirstName + rndNr.ToString()}, {iden.LastName + rndNr.ToString()}", admin.Account.UserID, Table);
        }
    }
}
