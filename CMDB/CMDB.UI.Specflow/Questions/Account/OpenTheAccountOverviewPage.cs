using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Questions.Account
{
    public class OpenTheAccountOverviewPage : Question<AccountOverviewPage>
    {
        public override AccountOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Account']");
            page.ClickElementByXpath("//a[@id='Account']");
            page.ClickElementByXpath("//a[@id='Account5']");
            page.ClickElementByXpath("//a[@href='/Account']");
            page.WaitOnAddNew();
            AccountOverviewPage accountOverview= WebPageFactory.Create<AccountOverviewPage>(page.WebDriver);
            return accountOverview;
        }
    }
}
