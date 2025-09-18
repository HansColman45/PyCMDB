package be.hans.cmdb.Questions.Account;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Account.AccountDetailsPage;
import be.cmdb.pages.Account.AccountOverviewPage;

/**
 * Question to open the Account Details Page from the Account Overview Page.
 */
public class OpenTheAccountDetailPage extends Question<AccountDetailsPage> {

    /**
     * Opens the Account Details Page from the Account Overview Page.
     * @param actor The actor performing the action.
     * @return The Account Details Page.
     */
    @Override
    public AccountDetailsPage performAs(IActor actor) {
        AccountOverviewPage ability = (AccountOverviewPage) actor.getAbility(AccountOverviewPage.class);
        ability.detailsButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(AccountDetailsPage.class, ability.getPage());
    }
}
