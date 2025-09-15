package be.hans.cmdb.Questions;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Identity.CreateIdentityPage;
import be.cmdb.pages.Identity.IdentityOverviewPage;

public class OpenTheCreateIdentityPage extends Question<CreateIdentityPage> {
    @Override
    public CreateIdentityPage performAs(IActor actor) {
        IdentityOverviewPage ability = (IdentityOverviewPage) actor.getAbility(IdentityOverviewPage.class);
        ability.newButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(CreateIdentityPage.class, ability.getPage());
    }
}
