package be.hans.cmdb;

import static org.assertj.core.api.Assertions.assertThat;
import org.junit.jupiter.api.Test;

import be.cmdb.helpers.IdentityHelper;
import be.cmdb.model.Identity;
import be.cmdb.pages.LoginPage;
import be.cmdb.pages.MainPage;
import be.cmdb.pages.identity.CreateIdentityPage;
import be.cmdb.pages.identity.EditIdentityPage;
import be.cmdb.pages.identity.IdentityDetailsPage;
import be.cmdb.pages.identity.IdentiyOverviewPage;

/**
 * Test class for Identity operations in the CMDB application.
 */
class IdentityTest extends BaseTest {
    /**
     * Tests the creation of a new identity in the CMDB application.
     * And check if we can find it back
     */
    @Test
    void canCreateNewIdentity() {
        Identity identity = IdentityHelper.createRandomIdentity();
        LoginPage loginPage = new LoginPage(getPage());
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

    @Test
    void canUpdateIdentity() {
        Identity identity = IdentityHelper.createRandomIdentity(getSession());
        LoginPage loginPage = new LoginPage(getPage());
        loginPage.navigate();
        MainPage mainPage = loginPage.login("Root", "796724Md");
        boolean isLoggedIn = loginPage.isUserLogedIn();
        assertThat(isLoggedIn).isTrue();
        IdentiyOverviewPage overviewPage = mainPage.openIdentityOverview();
        overviewPage.search(identity.getUserId());
        IdentityDetailsPage detailsPage = overviewPage.openIdentityDetails();
        EditIdentityPage editPage = detailsPage.openEditIdentity();
        editPage.setFirstName("UpdatedFirstName");
    }
}
