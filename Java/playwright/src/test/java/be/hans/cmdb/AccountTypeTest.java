package be.hans.cmdb;

//import be.cmdb.helpers.AccountTypeHelper;
//import be.cmdb.model.Type;
//import be.cmdb.pages.MainPage;
//import be.cmdb.pages.Type.CreateTypePage;
//import be.cmdb.pages.Type.DeleteTypePage;
//import be.cmdb.pages.Type.EditTypePage;
//import be.cmdb.pages.Type.TypeDetailPage;
//import be.cmdb.pages.Type.TypeOverviewPage;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

//import static org.assertj.core.api.Assertions.assertThat;

public class AccountTypeTest extends BaseTest{

    @Test
    void canCreateNewAccountType() {
//        Type type = AccountTypeHelper.createRandomAccountType();
//        MainPage mainPage = doLogin();
//        TypeOverviewPage typeOverviewPage = mainPage.openAccountTypeOverview();
//        CreateTypePage createTypePage = typeOverviewPage.openCreateTypePage();
//        createTypePage.setType(type.getType());
//        createTypePage.setDescription(type.getDescription());
//        createTypePage.clickCreateButton();
//        // Verify that the type was created successfully
//        typeOverviewPage.search(type.getType());
//        TypeDetailPage detailPage = typeOverviewPage.openTypeDetailPage();
//        String logline = detailPage.getLastLogline("accounttype");
//        assertThat(logline).contains("table accounttype")
//            .contains(type.getType())
//            .contains(type.getDescription())
//            .contains("by "+getAccount().getUserId());
    }

    @Test
    void canDeactivateAccountType(){
//        Type type = AccountTypeHelper.createDefaultAccountType(getSession(),getAdmin(), true);
//        MainPage mainPage = doLogin();
//        TypeOverviewPage typeOverviewPage = mainPage.openAccountTypeOverview();
//        typeOverviewPage.search(type.getType());
//        DeleteTypePage deleteTypePage = typeOverviewPage.openDeleteTypePage();
//        deleteTypePage.setReason("test");
//        deleteTypePage.deActivate();
//        // Verify that the type was deactivated successfully
//        typeOverviewPage.search(type.getType());
//        TypeDetailPage detailPage = typeOverviewPage.openTypeDetailPage();
//        String logline = detailPage.getLastLogline("accounttype");
//        assertThat(logline).contains("table accounttype")
//            .contains("test")
//            .contains("by "+getAccount().getUserId());
    }

    @Test
    void canActivateAnInactiveAccountType(){
//        Type type = AccountTypeHelper.createDefaultAccountType(getSession(),getAdmin(), false);
//        MainPage mainPage = doLogin();
//        TypeOverviewPage typeOverviewPage = mainPage.openAccountTypeOverview();
//        typeOverviewPage.search(type.getType());
//        typeOverviewPage.activate();
//        // Verify that the type was activated successfully
//        typeOverviewPage.search(type.getType());
//        TypeDetailPage detailPage = typeOverviewPage.openTypeDetailPage();
//        String logline = detailPage.getLastLogline("accounttype");
//        assertThat(logline).contains("table accounttype")
//            .contains("activated")
//            .contains("by "+getAccount().getUserId());
    }

    @DisplayName("Can update an accounttype for")
    @ParameterizedTest(name = "field:{0}, newValue:{1}")
    @CsvSource({
        "type, Alien",
        "description, Person from another planet"
    })
    void canUpdateAccountType(String field, String newValue) {
//        Type type = AccountTypeHelper.createDefaultAccountType(getSession(),getAdmin(), true);
//        String oldValue;
//        MainPage mainPage = doLogin();
//        TypeOverviewPage typeOverviewPage = mainPage.openAccountTypeOverview();
//        typeOverviewPage.search(type.getType());
//        EditTypePage editTypePage = typeOverviewPage.openEditTypePage();
//        // Update the specified field
//        switch (field) {
//            case "type":
//                oldValue = type.getType();
//                editTypePage.setType(newValue);
//                break;
//            case "description":
//                oldValue = type.getDescription();
//                editTypePage.setDescription(newValue);
//                break;
//            default:
//                throw new IllegalArgumentException("Unknown field: " + field);
//        }
//        editTypePage.edit();
//        // Verify that the update was successful
//        typeOverviewPage.search(newValue);
//        TypeDetailPage detailPage = typeOverviewPage.openTypeDetailPage();
//        String logline = detailPage.getLastLogline("accounttype");
//        assertThat(logline).contains("table accounttype")
//            .contains(field)
//            .contains(newValue)
//            .contains(oldValue)
//            .contains("by "+getAccount().getUserId());
    }
}
