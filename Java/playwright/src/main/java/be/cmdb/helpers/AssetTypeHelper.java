package be.cmdb.helpers;

import be.cmdb.builders.AssetTypeBuilder;
import be.cmdb.builders.LogBuilder;
import be.cmdb.dao.AssetTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Admin;
import be.cmdb.model.AssetType;
import be.cmdb.model.Category;
import be.cmdb.model.Log;
import org.hibernate.Session;
import org.hibernate.Transaction;
import java.util.List;

/**
 * Utility class for managing AssetType entities in the CMDB application.
 * Provides methods to delete an AssetType and its associated logs.
 */
public class AssetTypeHelper {

    /**
     * Creates a simple AssetType with default values for testing purposes.
     *
     * @param session the Hibernate session to use
     * @param category the Category to which the AssetType belongs
     * @param admin the Admin who will create the AssetType
     * @param active whether the AssetType is active or not
     * @return a new AssetType entity persisted in the database
     */
    public static AssetType createSimpleAssetType(Session session, Category category, Admin admin, boolean active) {
        Transaction transaction = session.beginTransaction();
        AssetType assetType = new AssetTypeBuilder()
            .withLastModifiedAdminId(admin.getAdminId())
            .withCategoryId(category.getId())
            .withActive(active ? 1 : 0)
            .build();
        AssetTypeDAO assetTypeDAO = new AssetTypeDAO();
        assetType = assetTypeDAO.save(session, assetType);

        Log log = new LogBuilder()
            .withAssetType(assetType)
            .withLogText("The " + category.getCategory() + "type Vendor: " + assetType.getVendor() +
                " and type: " + assetType.getType() + " is created by Automation in table assettype")
            .build();
        LogDAO  logDAO = new LogDAO();
        logDAO.save(session, log);

        transaction.commit();
        return assetType;
    }

    /**
     * Deletes an AssetType and all associated logs from the database.
     * @param session the Hibernate session to use for the transaction
     * @param type the AssetType to delete
     */
    public static void deleteAssetType(Session session, AssetType type) {
        Transaction transaction = session.beginTransaction();
        AssetTypeDAO typeDAO = new AssetTypeDAO();
        LogDAO logDAO = new LogDAO();

        // Delete all logs associated with the type
        List<Log> logs = logDAO.findByAssetTypeId(session, type.getTypeId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }

        // Delete the type itself
        typeDAO.delete(session, type);
        transaction.commit();
    }
}
