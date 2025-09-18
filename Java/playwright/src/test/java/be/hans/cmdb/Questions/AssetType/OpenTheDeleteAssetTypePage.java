package be.hans.cmdb.Questions.AssetType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.AssetType.AssetTypeOverviewPage;
import be.cmdb.pages.AssetType.DeactivateAssetTypePage;

/**
 * Question to open the Deactivate Asset Type page.
 */
public class OpenTheDeleteAssetTypePage extends Question<DeactivateAssetTypePage> {

    /**
     * Opens the Deactivate Asset Type page.
     * @param actor the actor performing the action
     * @return DeactivateAssetTypePage
     */
    @Override
    public DeactivateAssetTypePage performAs(IActor actor) {
        AssetTypeOverviewPage ability = (AssetTypeOverviewPage) actor.getAbility(AssetTypeOverviewPage.class);
        ability.deleteButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(DeactivateAssetTypePage.class, ability.getPage());
    }
}
