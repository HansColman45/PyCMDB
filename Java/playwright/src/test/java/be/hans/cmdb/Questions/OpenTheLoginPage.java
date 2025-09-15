package be.hans.cmdb.Questions;

import be.brightest.ScreenPlay.Abilities.WebPageFactory;
import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import be.cmdb.pages.LoginPage;
import com.microsoft.playwright.Browser;
import com.microsoft.playwright.BrowserContext;
import com.microsoft.playwright.BrowserType;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.Playwright;

import java.util.Arrays;

public class OpenTheLoginPage extends Question<LoginPage> {

    /**
     * Opens the login page and returns the LoginPage object.
     * @param actor the actor performing the action
     * @return the LoginPage object
     */
    @Override
    public LoginPage  performAs(IActor actor) {
        Playwright playwright = Playwright.create();
        Browser browser = playwright.chromium().launch(new BrowserType.LaunchOptions()
            .setHeadless(false)
            .setArgs(Arrays.asList("--start-maximized"))
        );
        BrowserContext context = browser.newContext();
        Page page = context.newPage();
        LoginPage loginPage = WebPageFactory.create(LoginPage.class, page);
        loginPage.navigate();
        return loginPage;
    }
}
