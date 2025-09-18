package be.hans.cmdb;

import be.cmdb.model.Admin;
import be.cmdb.model.Type;
import be.cmdb.pages.Type.TypeDetailPage;
import be.cmdb.pages.Type.TypeOverviewPage;
import be.hans.cmdb.Actors.IdentityTypeActor;
import be.hans.cmdb.Questions.IdentityType.OpenTheIdentityTypeDetailsPage;
import be.hans.cmdb.Questions.IdentityType.OpenTheIdentityTypeOverviewPage;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.assertj.core.api.Assertions.assertThat;

public class IdentityTypeTest extends BaseTest {

    @Test
    void canCreateNewIdentityType(){
        IdentityTypeActor typeActor = new IdentityTypeActor("IdentityTypeCreator");
        registerActor(typeActor);
        Admin admin = typeActor.createNewAdmin();
        Type type = typeActor.createIdentityType();
        typeActor.doLogin(typeActor.getAccount().getUserId(), defaultPassword());
        TypeOverviewPage typeOverviewPage = typeActor.asksFor(new OpenTheIdentityTypeOverviewPage());
        typeActor.doCreateIdentityType(type);
        // Verify that the type was created successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeActor.asksFor(new OpenTheIdentityTypeDetailsPage());
        String logline = detailPage.getLastLogline("identitytype");
        assertThat(logline).isNotEmpty()
            .isEqualToIgnoringCase(typeActor.getExpectedLogLine());
    }

    @Test
    void canDeactivateIdentityType(){
        IdentityTypeActor typeActor = new IdentityTypeActor("IdentityTypeDeactivator");
        registerActor(typeActor);
        Admin admin = typeActor.createNewAdmin();
        Type type = typeActor.createIdentityType(admin, true);
        typeActor.doLogin(typeActor.getAccount().getUserId(), defaultPassword());
        TypeOverviewPage typeOverviewPage = typeActor.asksFor(new OpenTheIdentityTypeOverviewPage());
        typeOverviewPage.search(type.getType());
        typeActor.doDeleteIdentityType(type, "test");
        // Verify that the type was deactivated successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeActor.asksFor(new OpenTheIdentityTypeDetailsPage());
        String logline = detailPage.getLastLogline("identitytype");
        assertThat(logline).isNotEmpty()
            .isEqualToIgnoringCase(typeActor.getExpectedLogLine());
    }

    @Test
    void canActivateAnInactiveIdentityType(){
        IdentityTypeActor typeActor = new IdentityTypeActor("IdentityTypeDeactivator");
        registerActor(typeActor);
        Admin admin = typeActor.createNewAdmin();
        Type type = typeActor.createIdentityType(admin, false);
        typeActor.doLogin(typeActor.getAccount().getUserId(), defaultPassword());
        TypeOverviewPage typeOverviewPage = typeActor.asksFor(new OpenTheIdentityTypeOverviewPage());
        typeOverviewPage.search(type.getType());
        typeActor.doActivateTheIdentityType(type);
        // Verify that the type was activated successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeActor.asksFor(new OpenTheIdentityTypeDetailsPage());
        String logline = detailPage.getLastLogline("identitytype");
        assertThat(logline).isNotEmpty()
            .isEqualToIgnoringCase(typeActor.getExpectedLogLine());
    }

    @DisplayName("Can update an identitytype for")
    @ParameterizedTest(name = "field:{0}, newValue:{1}")
    @CsvSource({
        "type, Alien",
        "description, Person from another planet"
    })
    void canEditIdentityType(String field, String newValue){
        IdentityTypeActor typeActor = new IdentityTypeActor("IdentityTypeUpdater");
        registerActor(typeActor);
        Admin admin = typeActor.createNewAdmin();
        Type type = typeActor.createIdentityType(admin, true);
        typeActor.doLogin(typeActor.getAccount().getUserId(), defaultPassword());
        TypeOverviewPage typeOverviewPage = typeActor.asksFor(new OpenTheIdentityTypeOverviewPage());
        typeOverviewPage.search(type.getType());
        type = typeActor.doUpdateIdentityType(type, field, newValue);
        // Verify that the update was successful
        typeOverviewPage.search(newValue);
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeActor.asksFor(new OpenTheIdentityTypeDetailsPage());
        String logline = detailPage.getLastLogline("identitytype");
        assertThat(logline).isNotEmpty()
            .isEqualToIgnoringCase(typeActor.getExpectedLogLine());
    }
}
