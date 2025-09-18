package be.hans.cmdb.Questions.Account;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Account.AccountOverviewPage;
import be.cmdb.pages.Account.EditAccountPage;

/**
 * Question to open the Edit Account Page from the Account Overview Page.
 */
public class OpenTheEditAccountPage extends Question<EditAccountPage> {

    /**
     * Opens the Edit Account Page from the Account Overview Page.
     * @param actor The actor performing the action.
     * @return The Edit Account Page.
     */
    @Override
    public EditAccountPage performAs(IActor actor) {
        AccountOverviewPage ability = (AccountOverviewPage) actor.getAbility(AccountOverviewPage.class);
        ability.editButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(EditAccountPage.class, ability.getPage());
    }
}
