package be.hans.cmdb.Questions.Account;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Account.AccountOverviewPage;
import be.cmdb.pages.MainPage;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

/**
 * Question to open the Account Overview page.
 */
public class OpenTheAccountOverviewPage extends Question<AccountOverviewPage> {

    /**
     * Performs the action of opening the Account Overview page.
     * @param actor The actor performing the action
     * @return AccountOverviewPage
     */
    @Override
    public AccountOverviewPage performAs(IActor actor) {
        MainPage ability = (MainPage) actor.getAbility(MainPage.class);
        ability.getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Account")).click();
        ability.getPage().locator("#Account5").click();
        ability.getPage().getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview")).click();
        ability.newButton().waitFor();
        return WebPageFactory.create(AccountOverviewPage.class, ability.getPage());
    }
}
