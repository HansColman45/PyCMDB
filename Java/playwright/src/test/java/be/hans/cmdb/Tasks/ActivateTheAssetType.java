package be.hans.cmdb.Tasks;

import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Tasks.Tasks;
import be.cmdb.pages.AssetType.AssetTypeOverviewPage;

/**
 * Task to activate the asset type.
 */
public class ActivateTheAssetType extends Tasks {

    /**
     * Task to activate the asset type.
     */
    @Override
    public void performAs(IActor actor) {
        AssetTypeOverviewPage ability = (AssetTypeOverviewPage) actor.getAbility(AssetTypeOverviewPage.class);
        ability.activate();
    }
}
