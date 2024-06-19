using CMDB.UI.Specflow.Abilities.Pages;
using CMDB.UI.Specflow.Actors;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public sealed class Hooks
    {
        /// <summary>
        /// The Nlog logger
        /// </summary>
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext context)
        {
            log.Debug("Scenario {0} started", context.ScenarioInfo.Title);
        }
        [AfterScenario]
        public void AfterScenario(ScenarioContext context)
        {
            log.Debug("Scenario {0} ended", context.ScenarioInfo.Title);
        }
        [AfterStep]
        public void AfterStep(ScenarioContext context)
        {
            var result = context.StepContext.Status;
            if (result == ScenarioExecutionStatus.TestError)
            {
                log.Error($"The scenario {context.ScenarioInfo.Title} on step {context.CurrentScenarioBlock} ended with {result}");
            }
        }
    }
}