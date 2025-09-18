package be.hans.cmdb.Questions.AccountType;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.MainPage;
import be.cmdb.pages.Type.TypeOverviewPage;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.AriaRole;

public class OpenTheAccountTypeOverviewPage extends Question<TypeOverviewPage> {
    @Override
    public TypeOverviewPage performAs(IActor actor) {
        MainPage ability = (MainPage) actor.getAbility(MainPage.class);
        ability.getPage().getByRole(AriaRole.BUTTON, new Page.GetByRoleOptions().setName("Types")).click();
        ability.getPage().locator("xpath=//a[@id='Account Type34']").click();
        ability.getPage().getByRole(AriaRole.LINK, new Page.GetByRoleOptions().setName("Overview")).click();
        ability.newButton().waitFor();
        return WebPageFactory.create(TypeOverviewPage.class, ability.getPage());
    }
}
