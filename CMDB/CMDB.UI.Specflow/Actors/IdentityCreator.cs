using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Questions.Identity;

namespace CMDB.UI.Specflow.Actors
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
            createPage.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_CreatePage");
            return createPage;
        }
        public void CreateNewIdentity(Helpers.Identity iden)
        {
            rndNr = rnd.Next();
            var page = GetAbility<CreateIdentityPage>();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_create");
            page.FirstName = iden.FirstName + rndNr.ToString();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_FirstName");
            page.LastName = iden.LastName + rndNr.ToString();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_LastName");
            page.Email = iden.Email;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Email");
            page.Company = iden.Company;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Company");
            page.UserId = iden.UserId + rndNr.ToString();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_UserId");
            page.Type = iden.Type;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Type");
            page.Language = iden.Language;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Language");
        }
        public void SearchIdentity(Helpers.Identity iden)
        {
            var page = GetAbility<IdentityOverviewPage>();
            page.Search(iden.FirstName + rndNr.ToString());
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = $"The Identity width name: {iden.FirstName + rndNr.ToString()}, {iden.LastName + rndNr.ToString()} is created by {admin.Account.UserID} in table identity";
        }
    }
}
