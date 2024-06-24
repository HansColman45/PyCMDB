using Bright.ScreenPlay.Actors;
using CMDB.UI.Specflow.Abilities.Pages;
using Bright.ScreenPlay.Questions;
using CMDB.UI.Specflow.Abilities.Pages.Types;

namespace CMDB.UI.Specflow.Questions.Main
{
    public class OpenTheAccountTypeOverviewPage : Question<TypeOverviewPage>
    {
        public override TypeOverviewPage PerformAs(IPerformer actor)
        {
            var page = actor.GetAbility<MainPage>();
            page.WaitUntilElmentVisableByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Types']");
            page.ClickElementByXpath("//a[@id='Account Type34']");
            page.ClickElementByXpath("//a[@href='/AccountType']");
            page.WaitOnAddNew();
            return new();
        }
    }
}
