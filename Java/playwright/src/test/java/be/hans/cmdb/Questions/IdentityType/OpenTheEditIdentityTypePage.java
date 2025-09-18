package be.hans.cmdb.Questions.IdentityType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Type.EditTypePage;
import be.cmdb.pages.Type.TypeOverviewPage;

/**
 * Question to open the Edit Identity Type page from the Identity Type Overview page.
 */
public class OpenTheEditIdentityTypePage extends Question<EditTypePage> {

    /**
     * Perform the action of opening the Edit IdentityType page.
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
