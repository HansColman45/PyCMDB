package be.hans.cmdb;

//import be.cmdb.dao.CategoryDAO;
//import be.cmdb.helpers.AssetTypeHelper;
//import be.cmdb.model.AssetType;
//import be.cmdb.model.Category;
//import be.cmdb.pages.AssetType.CreateAssetTypePage;
//import be.cmdb.pages.AssetType.AssetTypeDetailPage;
//import be.cmdb.pages.AssetType.AssetTypeOverviewPage;
//import be.cmdb.pages.AssetType.DeactivateAssetTypePage;
//import be.cmdb.pages.AssetType.EditAssetTypePage;
//import be.cmdb.pages.MainPage;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;

//import static org.assertj.core.api.Assertions.assertThat;

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
//        CategoryDAO categoryDAO = new CategoryDAO();
//        Category category = categoryDAO.findByCategory(getSession(),cat).get(0);
//        AssetType type = AssetTypeHelper.createRandomAssetType(category);
//        MainPage mainPage = doLogin();
//        AssetTypeOverviewPage overviewPage = mainPage.openAssetTypeOverviewPage();
//        CreateAssetTypePage createPage = overviewPage.openCreateAssetTypePage();
//        createPage.selectCategory(cat);
//        createPage.setType(type.getType());
//        createPage.setVendor(type.getVendor());
//        createPage.create();
//        //Verify that the type was created successfully
//        overviewPage.search(type.getType());
//        AssetTypeDetailPage detailPage = overviewPage.openDetailPage();
//        String logline = detailPage.getLastLogLine();
//        assertThat(logline).contains("table assettype")
//            .contains(cat)
//            .contains(type.getType())
//            .contains(type.getVendor())
//            .contains("by "+getAccount().getUserId());
    }

    @Test
    void canUpdateAssetType(){
//        CategoryDAO categoryDAO = new CategoryDAO();
//        Category category = categoryDAO.findAll(getSession()).get(0);
//        AssetType type = AssetTypeHelper.createSimpleAssetType(getSession(),category,getAdmin(),true);
//        MainPage mainPage = doLogin();
//        AssetTypeOverviewPage overviewPage = mainPage.openAssetTypeOverviewPage();
//        overviewPage.search(type.getType());
//        EditAssetTypePage editPage = overviewPage.openEditAssetTypePage();
//        String newtype = "Orange"+getRandomInt();
//        editPage.setType(newtype);
//        editPage.edit();
//        //Verify that the type was updated successfully
//        overviewPage.search("Orange");
//        AssetTypeDetailPage detailPage = overviewPage.openDetailPage();
//        String logline = detailPage.getLastLogLine();
//        assertThat(logline).contains("table assettype")
//            .contains("Type")
//            .contains(newtype)
//            .contains("by "+getAccount().getUserId());
    }

    @Test
    void canActivateAssetType(){
//        CategoryDAO categoryDAO = new CategoryDAO();
//        Category category = categoryDAO.findAll(getSession()).get(0);
//        AssetType type = AssetTypeHelper.createSimpleAssetType(getSession(),category,getAdmin(),false);
//        MainPage mainPage = doLogin();
//        AssetTypeOverviewPage overviewPage = mainPage.openAssetTypeOverviewPage();
//        overviewPage.search(type.getType());
//        overviewPage.activate();
//        //Verify that the type was activated successfully
//        overviewPage.search(type.getType());
//        AssetTypeDetailPage detailPage = overviewPage.openDetailPage();
//        String logline = detailPage.getLastLogLine();
//        assertThat(logline).contains("table assettype")
//            .contains("activated by " + getAccount().getUserId());
    }

    @Test
    void canDeactivateAssetType(){
//        CategoryDAO categoryDAO = new CategoryDAO();
//        Category category = categoryDAO.findAll(getSession()).get(0);
//        AssetType type = AssetTypeHelper.createSimpleAssetType(getSession(),category,getAdmin(),true);
//        MainPage mainPage = doLogin();
//        AssetTypeOverviewPage overviewPage = mainPage.openAssetTypeOverviewPage();
//        overviewPage.search(type.getType());
//        DeactivateAssetTypePage deactivate = overviewPage.openDeactivateAssetTypePage();
//        deactivate.setReason("Test");
//        deactivate.deActivate();
//        //Verify that the type was deactivated successfully
//        overviewPage.search(type.getType());
//        AssetTypeDetailPage detailPage = overviewPage.openDetailPage();
//        String logline = detailPage.getLastLogLine();
//        assertThat(logline).contains("table assettype")
//            .contains("deleted due to Test")
//            .contains("by " + getAccount().getUserId());
    }
}
