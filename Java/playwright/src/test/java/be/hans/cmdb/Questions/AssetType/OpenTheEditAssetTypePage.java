package be.hans.cmdb.Questions.AssetType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.AssetType.AssetTypeOverviewPage;
import be.cmdb.pages.AssetType.EditAssetTypePage;

/**
 * Question to open the Edit Asset Type page.
 */
public class OpenTheEditAssetTypePage extends Question<EditAssetTypePage> {

    /**
     * Opens the Edit Asset Type page.
     * @param actor the actor performing the action
     * @return EditAssetTypePage
     */
    @Override
    public EditAssetTypePage performAs(IActor actor) {
        AssetTypeOverviewPage ability = (AssetTypeOverviewPage) actor.getAbility(AssetTypeOverviewPage.class);
        ability.editButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(EditAssetTypePage.class, ability.getPage());
    }
}
