package be.hans.cmdb.Actors;

import be.cmdb.dao.CategoryDAO;
import be.cmdb.helpers.AssetTypeHelper;
import be.cmdb.helpers.LogLineHelper;
import be.cmdb.model.Admin;
import be.cmdb.model.AssetType;
import be.cmdb.model.Category;
import be.cmdb.pages.AssetType.CreateAssetTypePage;
import be.cmdb.pages.AssetType.DeactivateAssetTypePage;
import be.cmdb.pages.AssetType.EditAssetTypePage;
import be.hans.cmdb.Questions.AssetType.OpenTheCreateAssetTypePage;
import be.hans.cmdb.Questions.AssetType.OpenTheDeleteAssetTypePage;
import be.hans.cmdb.Questions.AssetType.OpenTheEditAssetTypePage;
import be.hans.cmdb.Tasks.ActivateTheAssetType;

public class AssetTypeActor extends CMDBActor{
    private final String table = "assettype";

    /**
     * Constructor for CMDBActor.
     * @param name the name of the actor
     */
    public AssetTypeActor(String name) {
        super(name);
    }

    /**
     * Create a random AssetType for the given category.
     * @param cat the category for which the asset type is created
     * @return AssetType the created asset type
     */
    public AssetType createRandomAssetType(String cat){
        CategoryDAO categoryDAO = new CategoryDAO();
        Category category = categoryDAO.findByCategory(getSession(),cat).get(0);
        return AssetTypeHelper.createRandomAssetType(category);
    }

    /**
     * Create a simple AssetType for the given category.
     * @param cat the category for which the asset type is created
     * @param admin the admin who creates the asset type
     * @param active whether the asset type is active or not
     * @return AssetType the created asset type
     */
    public AssetType createAssetType(String cat, Admin admin, boolean active){
        CategoryDAO categoryDAO = new CategoryDAO();
        Category category = categoryDAO.findByCategory(getSession(),cat).get(0);
        return AssetTypeHelper.createSimpleAssetType(getSession(), category, admin, active);
    }

    /**
     * Automate the creation of an asset type via the UI.
     * @param assetType the asset type to be created
     */
    public void doCreateAssetType(AssetType assetType){
        Category category = new CategoryDAO().findById(getSession(),assetType.getCategoryId());
        CreateAssetTypePage createTypePage = asksFor(new OpenTheCreateAssetTypePage());
        createTypePage.setType(assetType.getType());
        createTypePage.setVendor(assetType.getVendor());
        createTypePage.selectCategory(String.valueOf(assetType.getCategoryId()));
        String value = category.getCategory()+" type Vendor: "+assetType.getVendor()+" and type "+assetType.getType();
        setExpectedLogLine(LogLineHelper.createLogLine(value,getAccount().getUserId(),table));
        createTypePage.create();
    }

    /**
     * Automate the deletion of an asset type via the UI.
     * @param assetType the asset type to be deleted
     * @param reason the reason for deletion
     */
    public void doDeleteAssetType(AssetType assetType, String reason){
        Category category = new CategoryDAO().findById(getSession(),assetType.getCategoryId());
        DeactivateAssetTypePage deactivateAssetTypePage = asksFor(new OpenTheDeleteAssetTypePage());
        deactivateAssetTypePage.setReason(reason);
        deactivateAssetTypePage.deActivate();
        String value = category.getCategory()+" type Vendor: "+assetType.getVendor()+" and type "+assetType.getType();
        setExpectedLogLine(LogLineHelper.deleteLogLine(value,getAccount().getUserId(),reason,table));
    }

    /**
     * Automate the activation of an asset type via the UI.
     * @param assetType the asset type to be activated
     */
    public void doActivateAssetType(AssetType assetType){
        Category category = new CategoryDAO().findById(getSession(),assetType.getCategoryId());
        asksFor(new ActivateTheAssetType());
        String value = category.getCategory()+" type Vendor: "+assetType.getVendor()+" and type "+assetType.getType();
        setExpectedLogLine(LogLineHelper.activeLogLine(value,getAccount().getUserId(),table));
    }

    /**
     * Automate the update of an asset type via the UI.
     * @param assetType the asset type to be updated
     * @return AssetType the updated asset type
     */
    public AssetType doUpdateAssetType(AssetType assetType){
        EditAssetTypePage editPage = asksFor(new OpenTheEditAssetTypePage());
        String oldValue = assetType.getType();
        String newType = "Orange"+getRandomInt();
        assetType.setType(newType);
        editPage.setType(newType);
        editPage.edit();
        setExpectedLogLine(LogLineHelper.updateLogLine("Type",oldValue,newType,getAccount().getUserId(),table));
        return assetType;
    }
}
