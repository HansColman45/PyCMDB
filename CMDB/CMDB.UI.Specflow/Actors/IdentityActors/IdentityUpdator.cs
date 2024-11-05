using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Identity;

namespace CMDB.UI.Specflow.Actors.IdentityActors
{
    public class IdentityUpdator : IdentityActor
    {
        public IdentityUpdator(ScenarioContext scenarioContext, string name = "IdentityUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Identity> CreateNewIdentity(bool active = true)
        {
            if (active)
                return await Perform(new CreateTheIdentity());
            else
                return await Perform(new CreateTheInactiveIdentity());
        }
        public UpdateIdentityPage OpenUpdateIdentityPage()
        {
            var page = Perform(new OpenTheUpdateIdentityPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UpdatePage");
            return page;
        }
        public DeactivateIdentityPage OpenDeactivateIdentityPage()
        {
            var page = Perform(new OpenTheDeactivateIdentityPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            return page;
        }
        public Identity UpdateIdentity(string field, string value, Identity iden)
        {
            rndNr = rnd.Next();
            var page = GetAbility<UpdateIdentityPage>();
            switch (field)
            {
                case "FirstName":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, iden.FirstName, value + rndNr.ToString(), admin.Account.UserID, Table);
                    page.FirstName = value + rndNr.ToString();
                    iden.FirstName = value + rndNr.ToString();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_FirstName");
                    break;
                case "LastName":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, iden.LastName, value + rndNr.ToString(), admin.Account.UserID, Table);
                    page.LastName = value + rndNr.ToString();
                    iden.LastName = value + rndNr.ToString();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_LastName");
                    break;
                case "Company":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, iden.Company, value, admin.Account.UserID, Table);
                    page.Company = value;
                    iden.Company = value;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Company");
                    break;
                case "UserID":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, iden.UserID, value, admin.Account.UserID, Table);
                    page.UserId = value;
                    iden.UserID = value;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_UserId");
                    break;
                case "Email":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, iden.EMail, value, admin.Account.UserID, Table);
                    page.Email = value;
                    iden.EMail = value;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Email");
                    break;
                default:
                    log.Fatal($"Update on field {field} is not supported");
                    throw new Exception($"Update on field {field} is not supported");
            }
            page.Update();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Updated");
            return iden;
        }
        public void Deactivate(string reason, Identity identity)
        {
            var page = GetAbility<DeactivateIdentityPage>();
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_reason");
            page.Delete();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deleted");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Identity width name: {identity.Name}",admin.Account.UserID,reason,Table);
        }
        public void Activate(Identity identity)
        {
            var page = GetAbility<IdentityOverviewPage>();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Overview");
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Identity width name: {identity.Name}", admin.Account.UserID, Table);
        }
    }
}
