package be.hans.cmdb.Questions.Account;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Account.AccountOverviewPage;
import be.cmdb.pages.Account.CreateAccountPage;

/**
 * Question to open the Create Account Page from the Account Overview Page
 */
public class OpenTheCreateAccountPage extends Question<CreateAccountPage> {
    @Override
    public CreateAccountPage performAs(IActor actor) {
        AccountOverviewPage ability = (AccountOverviewPage) actor.getAbility(AccountOverviewPage.class);
        ability.newButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(CreateAccountPage.class, ability.getPage());
    }
}
