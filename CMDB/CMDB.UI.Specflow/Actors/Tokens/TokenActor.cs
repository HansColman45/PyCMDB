
using CMDB.UI.Specflow.Questions.Laptop;
using CMDB.UI.Specflow.Questions.Token;

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
                page.WebDriver = Driver;
                return page.GetLastLog();
            }
        }
    }
}
