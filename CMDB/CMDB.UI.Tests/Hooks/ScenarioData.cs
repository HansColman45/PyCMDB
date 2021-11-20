using CMDB.Domain.Entities;
using OpenQA.Selenium;
using CMDB.UI.Tests.Data;

namespace CMDB.UI.Tests.Hooks
{
    public class ScenarioData
    {
        public DataContext Context { get; set; }
        public Admin Admin { get; set; }
        public IWebDriver Driver { get; set; }
        public object User { get; internal set; }
    }
}
