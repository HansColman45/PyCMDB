package be.hans.cmdb.Questions;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Identity.DeleteIdentityPage;
import be.cmdb.pages.Identity.IdentityOverviewPage;

public class OpenDeleteIdentityPage extends Question<DeleteIdentityPage> {
    @Override
    public DeleteIdentityPage performAs(IActor actor) {
        IdentityOverviewPage ability = (IdentityOverviewPage) actor.getAbility(IdentityOverviewPage.class);
        ability.deleteButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(DeleteIdentityPage.class, ability.getPage());
    }
}
