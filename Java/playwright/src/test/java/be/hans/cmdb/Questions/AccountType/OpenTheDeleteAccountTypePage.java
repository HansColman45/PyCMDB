package be.hans.cmdb.Questions.AccountType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Type.DeleteTypePage;
import be.cmdb.pages.Type.TypeOverviewPage;

/**
 * Question to open the delete account type page.
 */
public class OpenTheDeleteAccountTypePage extends Question<DeleteTypePage> {

    /**
     * Question to open the delete account type page.
     * @param actor The actor performing the action
     * @return DeleteTypePage The delete account type page
     */
    @Override
    public DeleteTypePage performAs(IActor actor) {
        TypeOverviewPage ability = (TypeOverviewPage) actor.getAbility(TypeOverviewPage.class);
        ability.deleteButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(DeleteTypePage.class, ability.getPage());
    }
}
