using Bright.ScreenPlay.Actors;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Identity;
using OpenQA.Selenium;

namespace CMDB.UI.Specflow.Questions
{
    public class TheIdentityDertailLastLogLine : Question<string>
    {
        public override string PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<IdentityDetailPage>();
            page.ScrollToElement(By.XPath(IdentityDetailPage.LogOverviewXpath));
            string log = page.TekstFromElementByXpath("//td[contains(text(),'identity')]");
            return log;
        }
    }
}
