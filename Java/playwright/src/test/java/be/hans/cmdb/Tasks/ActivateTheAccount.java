package be.hans.cmdb.Tasks;

import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Tasks.Tasks;
import be.cmdb.pages.Account.AccountOverviewPage;

public class ActivateTheAccount extends Tasks {
    @Override
    public void performAs(IActor actor) {
        AccountOverviewPage ability = (AccountOverviewPage) actor.getAbility(AccountOverviewPage.class);
        ability.activateButton().click();
    }
}
