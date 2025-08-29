package be.cmdb.helpers;

import be.cmdb.builders.IdentityTypeBuilder;
import be.cmdb.builders.LogBuilder;
import be.cmdb.dao.IdentityTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Admin;
import be.cmdb.model.Log;
import be.cmdb.model.Type;
import org.hibernate.Session;
import org.hibernate.Transaction;
import java.util.List;

public class IdentityTypeHelper {

    /**
     * Creates a new identity type with default values for testing purposes.
     * @param session the Hibernate session to use
     * @param admin the Admin who will create the identity type
     * @param active whether the identity type should be active (1) or not (0)
     * @return a new Type entity persisted in the database
     */
    public static Type createIdentityType(Session session, Admin admin, boolean active) {
        Type type = new IdentityTypeBuilder()
            .withActive(active ? 1 : 0)
            .withLastModifiedAdminId(admin.getAdminId())
            .build();

        Transaction transaction = session.beginTransaction();
        IdentityTypeDAO identityTypeDAO = new IdentityTypeDAO();
        type = identityTypeDAO.save(session, type);

        Log log = new LogBuilder()
            .withType(type)
            .withLogText("IdentityType with type: " + type.getTypeId()
                + " and description: " + type.getDescription()
                +" is created by Automation in table identitytype")
            .build();
        LogDAO logDAO = new LogDAO();
        logDAO.save(session, log);
        transaction.commit();

        return type;
    }

    /***
     * Creates a random IdentityType object for testing purposes.
     * @return a new Type entity with random values
     */
    public static Type createRandomIdentityType() {
        return new IdentityTypeBuilder().build();
    }
    /**
     * Deletes an IdentityType and all associated logs from the database.
     * @param session the Hibernate session to use for the transaction
     * @param type the IdentityType to delete
     */
    public static void deleteType(Session session, Type type){
        Transaction transaction = session.beginTransaction();
        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByTypeId(session, type.getTypeId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        IdentityTypeDAO identityDAO = new IdentityTypeDAO();
        identityDAO.delete(session, type);
        transaction.commit();
    }
}
