using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.AccountPages;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Questions.Account
{
    public class TheAccountDertailLastLogLine : Question<string>
    {
        public override string PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<AccountDetailPage>();
            page.ScrollToElement(By.XPath("//h3[.='Log overview']"));
            return page.TekstFromElementByXpath("//td[contains(text(),'account')]");
        }
    }
}
