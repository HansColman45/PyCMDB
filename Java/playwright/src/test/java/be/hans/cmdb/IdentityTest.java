package be.hans.cmdb;

import static org.assertj.core.api.Assertions.assertThat;

import be.hans.cmdb.Questions.Identity.OpenTheIdentityDetailsPage;
import be.hans.cmdb.Questions.Identity.OpenTheIdentityOverviewPage;
import be.cmdb.model.Account;
import be.cmdb.model.Admin;
import be.cmdb.pages.Identity.IdentityDetailsPage;
import be.cmdb.pages.Identity.IdentityOverviewPage;
import be.hans.cmdb.Actors.IdentityActor;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import be.cmdb.model.Identity;

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
        IdentityActor identityActor = new IdentityActor("IdentityCreator");
        registerActor(identityActor);
        Admin admin = identityActor.createNewAdmin(identityActor.getSession());
        Account account = identityActor.getAccount();
        Identity identity = identityActor.getIdentity();
        identityActor.doLogin(account.getUserId(), defaultPassword());
        IdentityOverviewPage overviewPage = identityActor.asksFor(new OpenTheIdentityOverviewPage());
        identityActor.doCreateIdentity(identity);

        overviewPage.search(identity.getUserId());
        IdentityDetailsPage detailsPage = identityActor.asksFor(new OpenTheIdentityDetailsPage());
        String logLine = detailsPage.getLastLogline();
        assertThat(logLine).isNotEmpty()
            .isEqualToIgnoringCase(identityActor.getExpectedLogLine());
    }

    @DisplayName("Can update an identity for")
    @ParameterizedTest(name = "field:{0}, newValue:{1}")
    @CsvSource({
        "UserID,Test123",
        "FirstName,TestFirstName",
        "LastName,TestLastName",
        "EMail,test.test@CMDB.be"
    })
    void canUpdateIdentity(String field, String newValue) {
        String oldValue;
        newValue += getRandomInt(); // Ensure newValue is unique for the test
        IdentityActor identityActor = new IdentityActor("IdentityEditor");
        registerActor(identityActor);
        Admin admin = identityActor.createNewAdmin(identityActor.getSession());
        Account account = identityActor.getAccount();
        Identity identity = identityActor.getIdentity(identityActor.getSession(), admin, true);
        identityActor.doLogin(account.getUserId(), defaultPassword());
        IdentityOverviewPage overviewPage = identityActor.asksFor(new OpenTheIdentityOverviewPage());
        overviewPage.search(identity.getUserId());
        identity = identityActor.doUpdateIdenity(identity, field, newValue);
        overviewPage.search(identity.getUserId());
        // Verify that the identity was updated successfully
        IdentityDetailsPage detailsPage = identityActor.asksFor(new OpenTheIdentityDetailsPage());
        String logLine = detailsPage.getLastLogline();
        assertThat(logLine).isNotEmpty()
            .isEqualToIgnoringCase(identityActor.getExpectedLogLine());
    }

    @Test
    void canDeactivateIdentity() {
        IdentityActor identityActor = new IdentityActor("IdentityEditor");
        registerActor(identityActor);
        Admin admin = identityActor.createNewAdmin(identityActor.getSession());
        Account account = identityActor.getAccount();
        Identity identity = identityActor.getIdentity(identityActor.getSession(), admin, true);
        identityActor.doLogin(account.getUserId(), defaultPassword());
        IdentityOverviewPage overviewPage = identityActor.asksFor(new OpenTheIdentityOverviewPage());
        overviewPage.search(identity.getUserId());
        identityActor.doDeactivateIdentity(identity, "Test");
        // Verify that the identity is deactivated
        overviewPage.search(identity.getUserId());
        IdentityDetailsPage detailsPage = identityActor.asksFor(new OpenTheIdentityDetailsPage());
        String logLine = detailsPage.getLastLogline();
        assertThat(logLine).isNotEmpty()
            .isEqualToIgnoringCase(identityActor.getExpectedLogLine());
    }

    @Test
    void canActivateIdentity() {
        IdentityActor identityActor = new IdentityActor("IdentityEditor");
        registerActor(identityActor);
        Admin admin = identityActor.createNewAdmin(identityActor.getSession());
        Account account = identityActor.getAccount();
        Identity identity = identityActor.getIdentity(identityActor.getSession(), admin, false);
        identityActor.doLogin(account.getUserId(), defaultPassword());
        IdentityOverviewPage overviewPage = identityActor.asksFor(new OpenTheIdentityOverviewPage());
        overviewPage.search(identity.getUserId());
        identityActor.doActivateIdentity(identity);
        // Verify that the identity is activated
        overviewPage.search(identity.getUserId());
        IdentityDetailsPage detailsPage = overviewPage.openIdentityDetails();
        String logLine = detailsPage.getLastLogline();
        assertThat(logLine).contains("table identity")
            .contains("activated by " + account.getUserId());
    }
}
