using CMDB.Domain.Entities;
using Microsoft.Graph;
using Admin = CMDB.Domain.Entities.Admin;

namespace CMDB.UI.Specflow.StepDefinitions
{
    public class TestBase
    {
        /// <summary>
        /// The Nlog logger
        /// </summary>
        protected readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The expected log
        /// </summary>
        protected string expectedlog;
        /// <summary>
        /// The ScenatioContext
        /// </summary>
        protected ScenarioContext ScenarioContext { get; set; }
        /// <summary>
        /// The User that will login
        /// </summary>
        protected Admin Admin { get; set; }
        /// <summary>
        /// The ActorRegistry
        /// </summary>
        protected ActorRegistry ActorRegistry { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scenarioContext"></param>
        public TestBase(ScenarioContext scenarioContext, ActorRegistry actorRegistry)
        {
            ScenarioContext = scenarioContext;
            ActorRegistry = actorRegistry;
        }
    }
}
