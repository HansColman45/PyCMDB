using CMDB.Domain.Entities;
using CMDB.UI.Tests.Data;
using CMDB.UI.Tests.Hooks;
using System;

namespace CMDB.UI.Tests.Stepdefinitions
{
    public class TestBase
    {
        protected string Url;
        /// <summary>
        /// The Nlog logger
        /// </summary>
        protected readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        protected ScenarioData ScenarioData { get; set; }
        /// <summary>
        /// The connection to the database
        /// </summary>
        protected DataContext context;
        /// <summary>
        /// The User that will login
        /// </summary>
        protected Admin admin;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scenarioData"></param>
        public TestBase(ScenarioData scenarioData)
        {
            ScenarioData = scenarioData;
            context = scenarioData.Context;
            admin = scenarioData.Admin;
            ScenarioData.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }
    }
}
