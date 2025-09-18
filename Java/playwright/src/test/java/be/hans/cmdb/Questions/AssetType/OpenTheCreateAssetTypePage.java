package be.hans.cmdb.Questions.AssetType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.AssetType.AssetTypeOverviewPage;
import be.cmdb.pages.AssetType.CreateAssetTypePage;

/**
 * Question to open the Create Asset Type page.
 */
public class OpenTheCreateAssetTypePage extends Question<CreateAssetTypePage> {

    /**
     * Opens the Create Asset Type page.
     * @param actor the actor performing the action
     * @return CreateAssetTypePage
     */
    @Override
    public CreateAssetTypePage performAs(IActor actor) {
        AssetTypeOverviewPage ability = (AssetTypeOverviewPage) actor.getAbility(AssetTypeOverviewPage.class);
        ability.newButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(CreateAssetTypePage.class, ability.getPage());
    }
}
