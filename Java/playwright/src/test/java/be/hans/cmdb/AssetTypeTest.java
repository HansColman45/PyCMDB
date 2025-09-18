package be.hans.cmdb;

import be.cmdb.model.AssetType;
import be.cmdb.pages.AssetType.AssetTypeDetailPage;
import be.cmdb.pages.AssetType.AssetTypeOverviewPage;
import be.cmdb.model.Admin;
import be.hans.cmdb.Actors.AssetTypeActor;
import be.hans.cmdb.Questions.AssetType.OpenTheAssetTypeDetailPage;
import be.hans.cmdb.Questions.AssetType.OpenTheAssetTypeOverviewPage;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

import static org.assertj.core.api.Assertions.assertThat;

public class AssetTypeTest extends BaseTest{

    @ParameterizedTest(name = "category:{0}")
    @CsvSource({
        "Kensington",
        "Mobile",
        "Laptop",
        "Desktop",
        "Token",
        "Monitor",
        "Docking station"})
    void canCreateNewAssetType(String cat){
        AssetTypeActor assetTypeActor = new AssetTypeActor("AssetTypeCreator");
        registerActor(assetTypeActor);
        Admin admin = assetTypeActor.createNewAdmin();
        AssetType assetType = assetTypeActor.createRandomAssetType(cat);
        assetTypeActor.doLogin(assetTypeActor.getAccount().getUserId(), defaultPassword());
        AssetTypeOverviewPage overviewPage = assetTypeActor.asksFor(new OpenTheAssetTypeOverviewPage());
        assetTypeActor.doCreateAssetType(assetType);
        //Verify that the type was created successfully
        overviewPage.search(assetType.getType());
        AssetTypeDetailPage detailPage = assetTypeActor.asksFor(new OpenTheAssetTypeDetailPage());
        String logline = detailPage.getLastLogLine();
        assertThat(logline)
            .isNotEmpty()
            .isEqualToIgnoringCase(assetTypeActor.getExpectedLogLine());
    }

    @Test
    void canUpdateAssetType(){
        String cat = "Laptop";
        AssetTypeActor assetTypeActor = new AssetTypeActor("AssetTypeCreator");
        registerActor(assetTypeActor);
        Admin admin = assetTypeActor.createNewAdmin();
        AssetType assetType = assetTypeActor.createAssetType(cat, admin,true);
        assetTypeActor.doLogin(assetTypeActor.getAccount().getUserId(), defaultPassword());
        AssetTypeOverviewPage overviewPage = assetTypeActor.asksFor(new OpenTheAssetTypeOverviewPage());
        overviewPage.search(assetType.getType());
        assetType = assetTypeActor.doUpdateAssetType(assetType);
        //Verify that the type was updated successfully
        overviewPage.search(assetType.getType());
        AssetTypeDetailPage detailPage = assetTypeActor.asksFor(new OpenTheAssetTypeDetailPage());
        String logline = detailPage.getLastLogLine();
        assertThat(logline)
            .isNotEmpty()
            .isEqualToIgnoringCase(assetTypeActor.getExpectedLogLine());
    }

    @Test
    void canActivateAssetType(){
        String cat = "Laptop";
        AssetTypeActor assetTypeActor = new AssetTypeActor("AssetTypeCreator");
        registerActor(assetTypeActor);
        Admin admin = assetTypeActor.createNewAdmin();
        AssetType assetType = assetTypeActor.createAssetType(cat, admin,false);
        assetTypeActor.doLogin(assetTypeActor.getAccount().getUserId(), defaultPassword());
        AssetTypeOverviewPage overviewPage = assetTypeActor.asksFor(new OpenTheAssetTypeOverviewPage());
        overviewPage.search(assetType.getType());
        assetTypeActor.doActivateAssetType(assetType);
        overviewPage.search(assetType.getType());
        //Verify that the type was activated successfully
        AssetTypeDetailPage detailPage = assetTypeActor.asksFor(new OpenTheAssetTypeDetailPage());
        String logline = detailPage.getLastLogLine();
        assertThat(logline)
            .isNotEmpty()
            .isEqualToIgnoringCase(assetTypeActor.getExpectedLogLine());
    }

    @Test
    void canDeactivateAssetType(){
        String cat = "Laptop";
        AssetTypeActor assetTypeActor = new AssetTypeActor("AssetTypeCreator");
        registerActor(assetTypeActor);
        Admin admin = assetTypeActor.createNewAdmin();
        AssetType assetType = assetTypeActor.createAssetType(cat, admin,true);
        assetTypeActor.doLogin(assetTypeActor.getAccount().getUserId(), defaultPassword());
        AssetTypeOverviewPage overviewPage = assetTypeActor.asksFor(new OpenTheAssetTypeOverviewPage());
        overviewPage.search(assetType.getType());
        assetTypeActor.doDeleteAssetType(assetType, "Test");
        overviewPage.search(assetType.getType());
        //Verify that the type was deactivated successfully
        AssetTypeDetailPage detailPage = assetTypeActor.asksFor(new OpenTheAssetTypeDetailPage());
        String logline = detailPage.getLastLogLine();
        assertThat(logline)
            .isNotEmpty()
            .isEqualToIgnoringCase(assetTypeActor.getExpectedLogLine());
    }
}
