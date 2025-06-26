package be.hans.cmdb;
import static org.assertj.core.api.Assertions.*;

import java.util.List;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import com.microsoft.playwright.Browser;
import com.microsoft.playwright.BrowserContext;
import com.microsoft.playwright.BrowserType;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.Playwright;

import be.cmdb.helpers.IdentityHelper;
import be.cmdb.model.Identity;
import be.cmdb.pages.LoginPage;
import be.cmdb.pages.MainPage;
import be.cmdb.pages.identity.CreateIdentityPage;
import be.cmdb.pages.identity.IdentityDetailsPage;
import be.cmdb.pages.identity.IdentiyOverviewPage;

class BaseTest {
    static Playwright playwright;
    static Browser browser;

    // New instance for each test method.
    BrowserContext context;
    Page page;

    @BeforeAll
    static void launchBrowser() {
        playwright = Playwright.create();
        browser = playwright.chromium().launch(new BrowserType.LaunchOptions()
            .setHeadless(false)
            .setArgs(List.of("--start-maximized"))
        );
    }

    @AfterAll
    static void closeBrowser() {
        playwright.close();
    }

    @BeforeEach
    void createContextAndPage() {
        context = browser.newContext(new Browser.NewContextOptions().setViewportSize(null));
        page = context.newPage();
    }

    @AfterEach
    void closeContext() {
        context.close();
    }

    @Test
    void canLogin(){
        LoginPage loginPage = new LoginPage(page);
        loginPage.navigate();
        loginPage.login("Root", "796724Md");
        boolean isLoggedIn = loginPage.isUserLogedIn();
        assertThat(isLoggedIn).isTrue();
    }

    @Test
    void canCreateNewIdentity() {
        Identity identity = IdentityHelper.createRandomIdentity();
        
        LoginPage loginPage = new LoginPage(page);
        loginPage.navigate();
        MainPage mainPage = loginPage.login("Root", "796724Md");
        boolean isLoggedIn = loginPage.isUserLogedIn();
        assertThat(isLoggedIn).isTrue();
        
        IdentiyOverviewPage overviewPage = mainPage.openIdentityOverview();
        CreateIdentityPage createPage = overviewPage.openCreateIdentity();
        createPage.setfirstName(identity.getFirstName());
        createPage.setLastName(identity.getLastName());
        createPage.setEmail(identity.getEmail());
        createPage.setCompany(identity.getCompany());
        createPage.setUserId(identity.getUserId());
        createPage.selectType("4");
        createPage.selectLanguage("English");
        createPage.create();

        overviewPage.search(identity.getUserId());
        IdentityDetailsPage detailsPage = overviewPage.openIdentityDetails();
        String logLine = detailsPage.getLastLogline();
        assertThat(logLine).contains(identity.getFirstName())
                          .contains(identity.getLastName())
                          .contains("table identity");
    }   
}
