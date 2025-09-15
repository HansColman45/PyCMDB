package be.hans.cmdb.Questions;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.LoginPage;
import be.cmdb.pages.MainPage;
import com.microsoft.playwright.options.AriaRole;

public class OpenTheMainPage extends Question<MainPage> {

    /**
     * Opens the main page by clicking the login button on the login page and returns the MainPage object.
     * @param actor the actor performing the action
     * @return the MainPage object
     */
    @Override
    public MainPage performAs(IActor actor) {
        LoginPage loginPage = (LoginPage) actor.getAbility(LoginPage.class);
        loginPage.getPage().getByRole(AriaRole.BUTTON).click();
        loginPage.getPage().waitForLoadState();
        return WebPageFactory.create(MainPage.class, loginPage.getPage());
    }
}
