using Bright.ScreenPlay.Actors;
using CMDB.Domain.Entities;
using CMDB.UI.Specflow.Abilities.Data;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using CMDB.UI.Specflow.Questions;
using CMDB.UI.Specflow.Questions.Account;

namespace CMDB.UI.Specflow.Actors
{
    public class AccountUpdator : CMDBActor
    {
        public AccountUpdator(ScenarioContext scenarioContext, string name = "AccountUpdator") : base(scenarioContext, name)
        {
            IsAbleToDoOrUse<EditAccountPage>();
        }
        public async Task<Account> Account(bool active = false)
        {
            var db = GetAbility<DataContext>();
            return await db.CreateAccount(admin, active);
        }
        public EditAccountPage OpenEditAccountPage()
        {
            var page = Perform(new OpenTheAccountEditPage());
            page.WebDriver = Driver;
            page.TakeScreenShot($"{_scenarioContext.ScenarioInfo.Title}_{_scenarioContext.CurrentScenarioBlock}_EditPage");
            return page;
        }
    }
}
