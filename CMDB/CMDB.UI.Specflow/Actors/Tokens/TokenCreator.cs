using CMDB.Infrastructure;
using CMDB.UI.Specflow.Abilities.Pages.Token;
using CMDB.UI.Specflow.Questions.Token;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Tokens
{
    public class TokenCreator : TokenActor
    {
        public TokenCreator(ScenarioContext scenarioContext, string name = "TokenCreator") : base(scenarioContext, name)
        {
        }
        public async Task CreateNewToken(Helpers.Token token)
        {
            rndNr = rnd.Next();
            string Vendor, Type;
            Vendor = token.Type.Split(" ")[0];
            Type = token.Type.Split(" ")[1];
            var assetType = await GetOrCreateAssetType("Token", Vendor, Type);
            var page = Perform(new OpenTheTokenCreatePage());
            page.AssetTag = token.AssetTag + rndNr;
            page.SerialNumber = token.SerialNumber + rndNr;
            page.Type = $"{assetType.TypeID}";
            page.Create();
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Created");
        }
        public void SearchToken(Helpers.Token token) 
        {
            var page = GetAbility<TokenOverviewPage>();
            page.Search(token.AssetTag + rndNr);
            page.TakeScreenShot($"{ScenarioContext.ScenarioInfo.Title}_{ScenarioContext.CurrentScenarioBlock}_Search");
            ExpectedLog = GenericLogLineCreator.CreateLogLine($"Token with type {token.Type}", admin.Account.UserID, Table);
        }
    }
}
