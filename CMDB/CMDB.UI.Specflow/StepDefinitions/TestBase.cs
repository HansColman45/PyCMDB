using CMDB.Domain.Entities;

namespace CMDB.UI.Specflow.StepDefinitions
{
    public class TestBase
    {
        /// <summary>
        /// The Nlog logger
        /// </summary>
        protected readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The ScenatioContext
        /// </summary>
        protected ScenarioContext _scenarioContext { get; set; }
        /// <summary>
        /// The User that will login
        /// </summary>
        protected Admin Admin { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scenarioData"></param>
        /// <param name="scenarioContext"></param>
        public TestBase(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
    }
}
