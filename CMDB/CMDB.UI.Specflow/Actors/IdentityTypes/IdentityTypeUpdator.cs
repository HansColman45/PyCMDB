using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Types;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Types;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.IdentityTypes
{
    public class IdentityTypeUpdator : IdentityTypeActor
    {
        public IdentityTypeUpdator(ScenarioContext scenarioContext, string name = "IdentityTypeUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<IdentityType> CreateNewIdentityType(bool active = true)
        {
            if (active)
                return await Perform(new CreateTheIdentityType());
            else
                return await Perform(new CreateTheInactiveIdentityType());
        }
        public IdentityType UpdateIdentity(IdentityType identityType, string field, string value)
        {
            var editPage = Perform(new OpenTheTypeEditPage());
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
            switch (field)
            {
                case "Type":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, identityType.Type, value, admin.Account.UserID,Table);
                    editPage.Type = value;
                    editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
                    identityType.Type = value;
                    break;
                case "Description":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, identityType.Description, value, admin.Account.UserID, Table);
                    editPage.Description = value;
                    editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Description");
                    identityType.Description = value;
                    break;
                default:
                    log.Fatal($"The update for {field} is not implemented");
                    throw new Exception($"The update for {field} is not implemented");
            }
            editPage.Edit();
            return identityType;
        }
        public void DeactiveIdentityType(IdentityType identityType, string reason)
        {
            var editPage = Perform(new OpenTheTypeDeactivatePage());
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeactivatePage");
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Identitytype with type: {identityType.Type} and description: {identityType.Description}", admin.Account.UserID, reason, Table);
            editPage.Reason = reason;
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            editPage.Delete();
        }
        public void ActiveIdentityType(IdentityType identityType)
        {
            var editPage = GetAbility<TypeOverviewPage>();
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Identitytype with type: {identityType.Type} and description: {identityType.Description}", admin.Account.UserID, Table);
            editPage.Activate();
            editPage.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
    }
}
