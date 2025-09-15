package be.hans.cmdb.Questions;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.Identity.IdentityOverviewPage;
import be.cmdb.pages.MainPage;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

public class OpenTheIdentityOverviewPage extends Question<IdentityOverviewPage> {

    @Override
    public IdentityOverviewPage performAs(IActor actor) {
        MainPage ability = (MainPage) actor.getAbility(MainPage.class);
        ability.getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Identity")).click();
        ability.getPage().locator("#Identity2").click();
        ability.getPage().getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview")).click();
        ability.getPage().waitForLoadState();
        return WebPageFactory.create(IdentityOverviewPage.class, ability.getPage());
    }
}
