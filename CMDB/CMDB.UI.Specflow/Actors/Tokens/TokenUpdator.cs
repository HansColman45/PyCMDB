
using CMDB.Domain.Entities;
using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Token;
using CMDB.UI.Specflow.Questions.DataContextAnswers;
using CMDB.UI.Specflow.Questions.Monitor;
using CMDB.UI.Specflow.Questions.Token;

namespace CMDB.UI.Specflow.Actors.Tokens
{
    public class TokenUpdator : TokenActor
    {
        public TokenUpdator(ScenarioContext scenarioContext, string name = "TokenUpdator") : base(scenarioContext, name)
        {
        }
        public async Task<Token> CreateNewToken(bool active = true)
        {
            if (active)
                return await Perform(new CreateTheToken());
            else
                return await Perform(new CreateTheInactiveToken());
        }
        public async Task EditToken(Token token, string field, string newValue) 
        {
            rndNr = rnd.Next();
            switch (field)
            {
                case "SerialNumber":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field,token.SerialNumber, newValue+rndNr, admin.Account.UserID, Table);
                    var page = Perform(new OpenTheTokenEditPage());
                    page.WebDriver = Driver;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
                    page.SerialNumber = newValue + rndNr;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_SerialNumber");
                    page.Edit();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Edited");
                    break;
                case "Type":
                    ExpectedLog = GenericLogLineCreator.UpdateLogLine(field, $"{token.Type}",newValue ,admin.Account.UserID, Table);
                    string Vendor, Type;
                    Vendor = newValue.Split(" ")[0];
                    Type = newValue.Split(" ")[1];
                    var assetType = await GetOrCreateAssetType("Token", Vendor, Type);
                    page = Perform(new OpenTheTokenEditPage());
                    page.WebDriver = Driver;
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_EditPage");
                    page.Type = $"{assetType.TypeID}";
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Type");
                    page.Edit();
                    page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Edited");
                    break;
            }
        }
        public void DeactivateToken(Token token, string reason)
        {
            ExpectedLog = GenericLogLineCreator.DeleteLogLine($"Token with type {token.Type}", admin.Account.UserID, reason, Table);
            var page = Perform(new OpenTheTokenDeactivatePage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_DeletePage");
            page.Reason = reason;
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Reason");
            page.Delete();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Deactivated");
        }
        public void ActivateToken(Token token)
        {
            ExpectedLog = GenericLogLineCreator.ActivateLogLine($"Token with type {token.Type}", admin.Account.UserID, Table);
            var page = GetAbility<TokenOverviewPage>();
            page.Activate();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Activated");
        }
    }
}
