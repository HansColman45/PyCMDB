package be.hans.cmdb.Questions.Account;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Account.AccountOverviewPage;
import be.cmdb.pages.Account.DeleteAccountPage;

/**
 * Question to open the Delete Account Page from the Account Overview Page.
 */
public class OpenTheDeleteAccountPage extends Question<DeleteAccountPage> {

    /**
     * Method to perform the action of opening the Delete Account Page.
     * @param actor The actor performing the action
     * @return The Delete Account Page
     */
    @Override
    public DeleteAccountPage performAs(IActor actor) {
        AccountOverviewPage ability = (AccountOverviewPage) actor.getAbility(AccountOverviewPage.class);
        ability.deleteButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(DeleteAccountPage.class, ability.getPage());
    }
}
