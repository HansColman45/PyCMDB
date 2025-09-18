package be.hans.cmdb.Questions.AssetType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.AssetType.AssetTypeDetailPage;
import be.cmdb.pages.AssetType.AssetTypeOverviewPage;

/**
 * Question to open the Asset Type Detail Page by clicking the details button on the Asset Type Overview Page.
 */
public class OpenTheAssetTypeDetailPage extends Question<AssetTypeDetailPage> {

    /**
     * Navigate to the Asset Type Detail Page by clicking the details button on the Asset Type Overview Page.
     * @param actor the actor performing the action
     * @return AssetTypeDetailPage the detail page of the asset type
     */
    @Override
    public AssetTypeDetailPage performAs(IActor actor) {
        AssetTypeOverviewPage ability = (AssetTypeOverviewPage) actor.getAbility(AssetTypeOverviewPage.class);
        ability.detailsButton().click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(AssetTypeDetailPage.class, ability.getPage());
    }
}
