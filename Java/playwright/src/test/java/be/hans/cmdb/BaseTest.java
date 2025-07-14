package be.hans.cmdb;

import be.cmdb.pages.LoginPage;
import com.microsoft.playwright.Browser;
import com.microsoft.playwright.BrowserContext;
import com.microsoft.playwright.BrowserType;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.Playwright;
import java.util.List;
import java.util.Random;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.cfg.Configuration;
import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.assertj.core.api.Assertions.assertThat;

/**
 * Base class for Playwright tests, providing setup and teardown methods.
 */
class BaseTest {
    private static Playwright playwright;
    private static Browser browser;
    private BrowserContext context;
    private Page page;
    private Session session;
    private Random random = new Random();

    /**
     * Launches the browser before all tests.
     */
    @BeforeAll
    static void launchBrowser() {
        playwright = Playwright.create();
        browser = playwright.chromium().launch(new BrowserType.LaunchOptions()
            .setHeadless(false)
            .setArgs(List.of("--start-maximized"))
        );
    }

    /**
     * Closes the browser after all tests.
     */
    @AfterAll
    static void closeBrowser() {
        playwright.close();
    }

    /**
     * Creates a new browser context and page before each test.
     * Also initializes a Hibernate session for database operations.
     */
    @BeforeEach
    void createContextAndPage() {
        context = browser.newContext(new Browser.NewContextOptions().setViewportSize(null));
        page = context.newPage();
        SessionFactory sessionFactory = new Configuration().configure().buildSessionFactory();
        session = sessionFactory.openSession();
    }

    /**
     * Closes the browser context and page after each test.
     */
    @AfterEach
    void closeContext() {
        context.close();
        session.close();
    }

    /**
     * Test to verify that the login functionality works correctly.
     */
    @Test
    void canLogin() {
        LoginPage loginPage = new LoginPage(page);
        loginPage.navigate();
        loginPage.login("Root", "796724Md");
        boolean isLoggedIn = loginPage.isUserLogedIn();
        assertThat(isLoggedIn).isTrue();
    }

    /**
     * Returns the current Playwright Page object.
     * @return Current Playwright Page
     */
    public Page getPage() {
        return page;
    }

    /**
     * Sets the current Playwright Page object.
     * @param page
     */
    public void setPage(Page page) {
        this.page = page;
    }

    /**
     * Returns the current Hibernate Session object.
     * @return Current Hibernate Session
     */
    public Session getSession() {
        return session;
    }

    /**
     * This will return a random integer.
     * @return String representation of a random integer
     */
    protected String getRandomInt() {
        int rnd = random.nextInt(1000);
        return String.valueOf(rnd);
    }
}
