using CMDB.UI.Specflow.Abilities.Pages;
using Reqnroll;

namespace CMDB.UI.Specflow.StepDefinitions
{
    [Binding]
    public sealed class Hooks
    {
        /// <summary>
        /// The Nlog logger
        /// </summary>
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
       
        [BeforeScenario]
        public void BeforeScenario(ScenarioContext context, ActorRegistry actorRegistry)
        {
            log.Info("Scenario {0} started", context.ScenarioInfo.Title);
            actorRegistry.Clear();
        }
        [AfterScenario]
        public async Task AfterScenario(ScenarioContext context, ActorRegistry actorRegistry)
        {
            log.Info("Scenario {0} ended", context.ScenarioInfo.Title);
            try
            {
                await actorRegistry.DisposeActors();
            }
            catch (Exception e)
            {
                log.Fatal(e.Message);
                throw;
            }
        }
        [AfterStep]
        public void AfterStep(ScenarioContext context, ActorRegistry actorRegistry)
        {
            var result = context.StepContext.Status;
            if (result == ScenarioExecutionStatus.TestError)
            {
                var errorMessage = context.TestError.Message;
                log.Error($"The scenario {context.ScenarioInfo.Title} on step {context.CurrentScenarioBlock} ended with {result} and the error message is {errorMessage}");
                var actor = actorRegistry.Actors.First();
                var page = actor.GetAbility<MainPage>();
                page.Settings.TakeScreenShot = true;
                page.TakeScreenShot($"{context.ScenarioInfo.Title}_{context.CurrentScenarioBlock}_Error");
                page.Settings.TakeScreenShot = false;
                log.Info("A screenshot is saved");
            }
        }
        
    }
}