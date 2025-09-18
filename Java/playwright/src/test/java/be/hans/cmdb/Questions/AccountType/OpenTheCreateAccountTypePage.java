package be.hans.cmdb.Questions.AccountType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Type.CreateTypePage;
import be.cmdb.pages.Type.TypeOverviewPage;

/**
 * Question to open the Create Account Type page from the Account Type Overview page.
 */
public class OpenTheCreateAccountTypePage extends Question<CreateTypePage> {

    /**
     * Perform the action of opening the Create Account Type page.
     * @param actor The actor performing the action.
     * @return An instance of CreateTypePage representing the opened page.
     */
    @Override
    public CreateTypePage performAs(IActor actor) {
        TypeOverviewPage ability = (TypeOverviewPage) actor.getAbility(TypeOverviewPage.class);
        ability.newButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(CreateTypePage.class, ability.getPage());
    }
}
