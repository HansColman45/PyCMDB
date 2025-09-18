package be.hans.cmdb.Questions.AccountType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Type.TypeDetailPage;
import be.cmdb.pages.Type.TypeOverviewPage;

/**
 * Question to open the Account Type Detail Page from the Account Type Overview Page.
 */
public class OpenTheAccountTypeDetailPage extends Question<TypeDetailPage> {

    /**
     * Question to open the Account Type Detail Page from the Account Type Overview Page.
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
