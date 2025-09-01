package be.hans.cmdb;

import be.cmdb.helpers.IdentityTypeHelper;
import be.cmdb.model.Type;
import be.cmdb.pages.MainPage;
import be.cmdb.pages.Type.*;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.assertj.core.api.Assertions.assertThat;

public class IdentityTypeTest extends BaseTest {

    @Test
    void canCreateNewIdentityType(){
        Type type = IdentityTypeHelper.createRandomIdentityType();
        MainPage mainPage = doLogin();
        //Create new Identity Type
        TypeOverviewPage typeOverviewPage = mainPage.openIdentityTypeOverviewPage();
        CreateTypePage createTypePage = typeOverviewPage.openCreateTypePage();
        createTypePage.setType(type.getType());
        createTypePage.setDescription(type.getDescription());
        createTypePage.clickCreateButton();
        // Verify that the type was created successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeOverviewPage.openTypeDetailPage();
        String logline = detailPage.getLastLogline("identitytype");
        assertThat(logline).contains("table identitytype")
            .contains(type.getType())
            .contains(type.getDescription())
            .contains("by "+getAccount().getUserId());
    }

    @Test
    void canDeactivateIdentityType(){
        //First create a new type in the Db
        Type type = IdentityTypeHelper.createIdentityType(getSession(),getAdmin(), true);
        MainPage mainPage = doLogin();
        TypeOverviewPage typeOverviewPage = mainPage.openIdentityTypeOverviewPage();
        typeOverviewPage.search(type.getType());
        //Delete Identity Type
        DeleteTypePage deleteTypePage = typeOverviewPage.openDeleteTypePage();
        deleteTypePage.setReason("test");
        deleteTypePage.deActivate();
        // Verify that the type was deactivated successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeOverviewPage.openTypeDetailPage();
        String logline = detailPage.getLastLogline("identitytype");
        assertThat(logline).contains("table identitytype")
            .contains("test")
            .contains("by "+getAccount().getUserId());
    }

    @Test
    void canActivateAnInactiveIdentityType(){
        //First create a new type in the Db
        Type type = IdentityTypeHelper.createIdentityType(getSession(),getAdmin(), false);
        MainPage mainPage = doLogin();
        TypeOverviewPage typeOverviewPage = mainPage.openIdentityTypeOverviewPage();
        typeOverviewPage.search(type.getType());
        //Activate Identity Type
        typeOverviewPage.activate();
        // Verify that the type was activated successfully
        typeOverviewPage.search(type.getType());
        TypeDetailPage detailPage = typeOverviewPage.openTypeDetailPage();
        String logline = detailPage.getLastLogline("identitytype");
        assertThat(logline).contains("table identitytype")
            .contains("activated by "+getAccount().getUserId());
    }

    @DisplayName("Can update an identitytype for")
    @ParameterizedTest(name = "field:{0}, newValue:{1}")
    @CsvSource({
        "type, Alien",
        "description, Person from another planet"
    })
    void canEditIdentityType(String field, String newValue){
        //First create a new type in the Db
        Type type = IdentityTypeHelper.createIdentityType(getSession(),getAdmin(), true);
        String oldValue;
        newValue += getRandomInt();
        //Find it in the app and activate it
        MainPage mainPage = doLogin();
        TypeOverviewPage typeOverviewPage = mainPage.openIdentityTypeOverviewPage();
        typeOverviewPage.search(type.getType());
        //Update
        EditTypePage editTypePage = typeOverviewPage.openEditTypePage();
        switch (field){
            case "type":
                oldValue = type.getType();
                editTypePage.setType(newValue);
                break;
            case "description":
                oldValue = type.getDescription();
                editTypePage.setDescription(newValue);
                break;
            default:
                throw new IllegalArgumentException("Invalid field: " + field);
        }
        editTypePage.edit();
        // Verify that the update was successful
        typeOverviewPage.search(newValue);
        TypeDetailPage detailPage = typeOverviewPage.openTypeDetailPage();
        String logline = detailPage.getLastLogline("identitytype");
        assertThat(logline).contains("table identitytype")
            .contains(field)
            .contains(newValue)
            .contains(oldValue)
            .contains("by "+getAccount().getUserId());
    }
}
