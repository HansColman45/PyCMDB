using CMDB.UI.Specflow.Questions.Token;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Tokens
{
    public class TokenActor : CMDBActor
    {
        protected static string Table => "token";
        public TokenActor(ScenarioContext scenarioContext, string name = "TokenActor") : base(scenarioContext, name)
        {
        }
        public string LastLogLine
        {
            get
            {
                var page = Perform(new OpenTheTokenDetailPage());
                return page.GetLastLog();
            }
        }
    }
}
