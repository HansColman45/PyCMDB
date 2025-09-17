package be.hans.cmdb.Questions.Identity;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Identity.EditIdentityPage;
import be.cmdb.pages.Identity.IdentityOverviewPage;

public class OpenTheEditIdentityPage extends Question<EditIdentityPage> {
    @Override
    public EditIdentityPage performAs(IActor actor) {
        IdentityOverviewPage overviewPage = (IdentityOverviewPage) actor.getAbility(IdentityOverviewPage.class);
        overviewPage.editButton().click();
        overviewPage.getPage().waitForLoadState();
        return WebPageFactory.create(EditIdentityPage.class, overviewPage.getPage());
    }
}
