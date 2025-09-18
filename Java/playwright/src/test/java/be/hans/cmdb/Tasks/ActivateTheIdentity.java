package be.hans.cmdb.Tasks;

import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Tasks.Tasks;
import be.cmdb.pages.Identity.IdentityOverviewPage;

/**
 * Task to activate an identity in the CMDB application.
 */
public class ActivateTheIdentity extends Tasks {

    @Override
    public void performAs(IActor actor) {
        IdentityOverviewPage ability = (IdentityOverviewPage) actor.getAbility(IdentityOverviewPage.class);
        ability.activate();
    }
}
