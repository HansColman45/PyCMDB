package be.hans.cmdb;

import be.cmdb.model.Admin;
import be.cmdb.model.Type;
import be.cmdb.pages.Type.TypeDetailPage;
import be.cmdb.pages.Type.TypeOverviewPage;
import be.hans.cmdb.Actors.AccountTypeActor;
import be.hans.cmdb.Questions.AccountType.OpenTheAccountTypeDetailPage;
import be.hans.cmdb.Questions.AccountType.OpenTheAccountTypeOverviewPage;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.assertj.core.api.Assertions.assertThat;

public class AccountTypeTest extends BaseTest{

    @Test
    void canCreateNewAccountType() {
        AccountTypeActor typeActor = new AccountTypeActor("AccountTypeCreator");
        registerActor(typeActor);
        typeActor.createNewAdmin();
        Type type = typeActor.createAccountType();
        typeActor.doLogin(typeActor.getAccount().getUserId(), defaultPassword());
        TypeOverviewPage typeOverviewPage = typeActor.asksFor(new OpenTheAccountTypeOverviewPage());
        typeActor.doCreateAccountType(typeActor.createAccountType());
        // Verify that the type was created successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeActor.asksFor(new OpenTheAccountTypeDetailPage());
        String logLine = detailPage.getLastLogline("accounttype");
        assertThat(logLine).isNotEmpty()
            .isEqualToIgnoringCase(typeActor.getExpectedLogLine());
    }

    @Test
    void canDeactivateAccountType(){
        AccountTypeActor typeActor = new AccountTypeActor("AccountTypeDeactivator");
        registerActor(typeActor);
        Admin admin = typeActor.createNewAdmin();
        Type type = typeActor.createAccountType(admin, true);
        typeActor.doLogin(typeActor.getAccount().getUserId(), defaultPassword());
        TypeOverviewPage typeOverviewPage = typeActor.asksFor(new OpenTheAccountTypeOverviewPage());
        typeOverviewPage.search(type.getType());
        typeActor.doDeleteAccountType(type,"Test");
        // Verify that the type was deactivated successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeActor.asksFor(new OpenTheAccountTypeDetailPage());
        String logline = detailPage.getLastLogline("accounttype");
        assertThat(logline)
            .isNotEmpty()
            .isEqualToIgnoringCase(typeActor.getExpectedLogLine());
    }

    @Test
    void canActivateAnInactiveAccountType(){
        AccountTypeActor typeActor = new AccountTypeActor("AccountTypeDeactivator");
        registerActor(typeActor);
        Admin admin = typeActor.createNewAdmin();
        Type type = typeActor.createAccountType(admin, false);
        typeActor.doLogin(typeActor.getAccount().getUserId(), defaultPassword());
        TypeOverviewPage typeOverviewPage = typeActor.asksFor(new OpenTheAccountTypeOverviewPage());
        typeOverviewPage.search(type.getType());
        typeActor.doActivateAccountType(type);
        // Verify that the type was activated successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeActor.asksFor(new OpenTheAccountTypeDetailPage());
        String logline = detailPage.getLastLogline("accounttype");
        assertThat(logline)
            .isNotEmpty()
            .isEqualToIgnoringCase(typeActor.getExpectedLogLine());
    }

    @DisplayName("Can update an accounttype for")
    @ParameterizedTest(name = "field:{0}, newValue:{1}")
    @CsvSource({
        "type, Alien",
        "description, Person from another planet"
    })
    void canUpdateAccountType(String field, String newValue) {
        AccountTypeActor typeActor = new AccountTypeActor("AccountTypeUpdater");
        registerActor(typeActor);
        Admin admin = typeActor.createNewAdmin();
        Type type = typeActor.createAccountType(admin, true);
        typeActor.doLogin(typeActor.getAccount().getUserId(), defaultPassword());
        TypeOverviewPage typeOverviewPage = typeActor.asksFor(new OpenTheAccountTypeOverviewPage());
        typeOverviewPage.search(type.getType());
        type = typeActor.doUpdateAccountType(type, field, newValue);
        // Verify that the type was updated successfully
        typeOverviewPage.search(newValue);
        TypeDetailPage detailPage = typeActor.asksFor(new OpenTheAccountTypeDetailPage());
        String logline = detailPage.getLastLogline("accounttype");
        assertThat(logline)
            .isNotEmpty()
            .isEqualToIgnoringCase(typeActor.getExpectedLogLine());
    }
}
