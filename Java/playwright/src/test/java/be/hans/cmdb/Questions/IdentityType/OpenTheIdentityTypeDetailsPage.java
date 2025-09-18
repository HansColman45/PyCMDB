package be.hans.cmdb.Questions.IdentityType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Type.TypeDetailPage;
import be.cmdb.pages.Type.TypeOverviewPage;

/**
 * Question to open the Identity Type Detail Page from the Identity Type Overview Page.
 */
public class OpenTheIdentityTypeDetailsPage extends Question<TypeDetailPage> {

    /**
     * Question to open the Identity Type Detail Page from the Identity Type Overview Page.
     * @param actor the actor performing the action
     * @return TypeDetailPage
     */
    @Override
    public TypeDetailPage performAs(IActor actor) {
        TypeOverviewPage ability = (TypeOverviewPage) actor.getAbility(TypeOverviewPage.class);
        ability.detailsButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(TypeDetailPage.class, ability.getPage());
    }
}
