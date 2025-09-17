package be.hans.cmdb.Questions.Identity;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Identity.IdentityDetailsPage;
import be.cmdb.pages.Identity.IdentityOverviewPage;

public class OpenTheIdentityDetailsPage extends Question<IdentityDetailsPage> {
    @Override
    public IdentityDetailsPage performAs(IActor actor) {
        IdentityOverviewPage ability = (IdentityOverviewPage) actor.getAbility(IdentityOverviewPage.class);
        ability.detailsButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(IdentityDetailsPage.class, ability.getPage());
    }
}
