package be.cmdb.helpers;

import be.cmdb.dao.AssetTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.AssetType;
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
     * Deletes an AssetType and all associated logs from the database.
     * @param session the Hibernate session to use for the transaction
     * @param type the AssetType to delete
     */
    public static void deleteAssetType(Session session, AssetType type) {
        Transaction transaction = session.beginTransaction();
        AssetTypeDAO typeDAO = new AssetTypeDAO();
        LogDAO logDAO = new LogDAO();

        // Delete all logs associated with the type
        List<Log> logs = logDAO.findbyAssetTypeId(session, type.getTypeId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }

        // Delete the type itself
        typeDAO.delete(session, type);

        transaction.commit();
    }
}
