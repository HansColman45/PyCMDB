package be.hans.cmdb.Tasks;

import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Tasks.Tasks;
import be.cmdb.pages.AssetType.AssetTypeOverviewPage;
import be.cmdb.pages.Type.TypeOverviewPage;

/**
 * Task to activate the selected identity type.
 */
public class ActivateTheIdentityType extends Tasks {

    /**
     * Activate the selected account type.
     * @param actor The actor performing the task
     */
    @Override
    public void performAs(IActor actor) {
        TypeOverviewPage ability = (TypeOverviewPage) actor.getAbility(AssetTypeOverviewPage.class);
        ability.activateButton().click();
    }
}
