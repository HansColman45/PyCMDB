using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Questions.Mobile;

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
