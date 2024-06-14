using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Questions;

namespace CMDB.UI.Specflow.Actors
{
    public class IdentityUpdator : CMDBActor
    {
        public IdentityUpdator(ScenarioContext scenarioContext, string name = "IdentityUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Identity> CreateNewIdentity()
        {
            var context = GetAbility<DataContext>();
            return await context.CreateIdentity(admin);
        }
        public Identity UpdateIdentity(string field, string value, Identity iden)
        {
            rndNr = rnd.Next();
            var page = GetAbility<UpdateIdentityPage>();
            switch (field)
            {
                case "FirstName":
                    ExpectedLog = $"The {field} has been changed from {iden.FirstName} to {value + rndNr.ToString()} by {admin.Account.UserID} in table identity";
                    page.FirstName = value + rndNr.ToString();
                    iden.FirstName = value + rndNr.ToString();
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_FirstName");
                    break;
                case "LastName":
                    ExpectedLog = $"The {field} has been changed from {iden.LastName} to {value + rndNr.ToString()} by {admin.Account.UserID} in table identity";
                    page.LastName = value + rndNr.ToString();
                    iden.LastName = value + rndNr.ToString();
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_LastName");
                    break;
                case "Company":
                    ExpectedLog = $"The {field} has been changed from {iden.Company} to {value} by {admin.Account.UserID} in table identity";
                    page.Company = value;
                    iden.Company = value;
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Company");
                    break;
                case "UserID":
                    ExpectedLog = $"The {field} has been changed from {iden.UserID} to {value} by {admin.Account.UserID} in table identity";
                    page.UserId = value;
                    iden.UserID = value;
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_UserId");
                    break;
                case "Email":
                    ExpectedLog = $"The {field} has been changed from {iden.EMail} to {value} by {admin.Account.UserID} in table identity";
                    page.Email = value;
                    iden.EMail = value;
                    page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Email");
                    break;
                default:
                    throw new Exception($"Field {field} is not supported");
            }
            page.Update();
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_Updated");
            return iden;
        }
        public string LastLogLine
        {
            get
            {
                var detail = Perform(new OpenTheIdentityDetailPage());
                detail.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_detail");
                return detail.GetLastLog();
            }
        }
    }
}
