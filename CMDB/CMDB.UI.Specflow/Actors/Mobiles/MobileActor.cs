using CMDB.UI.Specflow.Questions.Mobile;
using Reqnroll;

namespace CMDB.UI.Specflow.Actors.Mobiles
{
    public class MobileActor : CMDBActor
    {
        protected static string Table => "mobile";
        public MobileActor(ScenarioContext scenarioContext, string name = "MobileActor") : base(scenarioContext, name)
        {
        }
        public string GetLastMobileLogLine
        {
            get
            {
                var page = Perform(new OpenTheMobileDetailPage());
                return page.GetLastLog();
            }
        }
    }
}
