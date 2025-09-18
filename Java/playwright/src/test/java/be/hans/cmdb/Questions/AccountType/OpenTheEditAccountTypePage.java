package be.hans.cmdb.Questions.AccountType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Type.EditTypePage;
import be.cmdb.pages.Type.TypeOverviewPage;

/**
 * Question to open the Edit Account Type page from the Account Type Overview page.
 */
public class OpenTheEditAccountTypePage extends Question<EditTypePage> {
    /**
     * Perform the action of opening the Edit Account Type page.
     * @param actor the actor who is asking the question
     * @return the EditTypePage
     */
    @Override
    public EditTypePage performAs(IActor actor) {
        TypeOverviewPage overviewPage = (TypeOverviewPage) actor.getAbility(TypeOverviewPage.class);
        overviewPage.editButton().click();
        overviewPage.getPage().waitForLoadState();
        return WebPageFactory.create(EditTypePage.class, overviewPage.getPage());
    }
}
