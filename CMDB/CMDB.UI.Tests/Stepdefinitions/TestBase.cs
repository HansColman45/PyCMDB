using CMDB.Domain.Entities;
using CMDB.Testing.Helpers;
using CMDB.UI.Tests.Data;
using CMDB.UI.Tests.Hooks;
using System;
using TechTalk.SpecFlow;

namespace CMDB.UI.Tests.Stepdefinitions
{
    public class TestBase
    {
        protected string Url;
        /// <summary>
        /// The Nlog logger
        /// </summary>
        protected readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The ScenarioData
        /// </summary>
        protected ScenarioData ScenarioData { get; set; }
        /// <summary>
        /// The ScenatioContext
        /// </summary>
        protected ScenarioContext ScenarioContext { get; set; }
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
        /// <param name="scenarioContext"></param>
        public TestBase(ScenarioData scenarioData, ScenarioContext scenarioContext)
        {
            ScenarioData = scenarioData;
            context = scenarioData.Context;
            admin = scenarioData.Admin;
            ScenarioData.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            ScenarioContext = scenarioContext; 
        }
    }
}
