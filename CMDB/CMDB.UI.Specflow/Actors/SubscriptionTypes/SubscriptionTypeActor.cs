using CMDB.UI.Specflow.Questions.SubscriptionType;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.SubscriptionTypes
{
    public class SubscriptionTypeActor : CMDBActor
    {
        protected static string Table => "subscriptiontype";
        public SubscriptionTypeActor(ScenarioContext scenarioContext, string name = "SubscriptionType") : base(scenarioContext, name)
        {
        }
        public string LastLogLine
        {
            get
            {
                var page = Perform(new OpenTheSubscriptionTypeDetailPage());
                return page.GetLastLog();
            }
        }

    }
}
