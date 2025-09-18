package be.hans.cmdb.Questions.Identity;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Identity.DeleteIdentityPage;
import be.cmdb.pages.Identity.IdentityOverviewPage;

/**
 * Question to open the Delete Identity Page from the Identity Overview Page.
 */
public class OpenDeleteIdentityPage extends Question<DeleteIdentityPage> {

    /**
     * Opens the Delete Identity Page from the Identity Overview Page.
     * @param actor The actor performing the action.
     * @return The Delete Identity Page.
     */
    @Override
    public DeleteIdentityPage performAs(IActor actor) {
        IdentityOverviewPage ability = (IdentityOverviewPage) actor.getAbility(IdentityOverviewPage.class);
        ability.deleteButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(DeleteIdentityPage.class, ability.getPage());
    }
}
