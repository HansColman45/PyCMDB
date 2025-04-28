using CMDB.UI.Specflow.Questions.Subscription;

namespace CMDB.UI.Specflow.Actors.Subscriptions
{
    public class SubscriptionActor : CMDBActor
    {
        public SubscriptionActor(ScenarioContext scenarioContext, string name = "SubscriptionActor") : base(scenarioContext, name)
        {
        }
        protected static string Table => "subscription";

        public string LastLogLine
        {
            get
            {
                var page = Perform(new OpenTheSubscriptionDetailPage());
                return page.GetLastLog();
            }
        }
    }
}
