package be.cmdb.helpers;

import java.util.List;

import be.cmdb.model.Admin;
import org.hibernate.Session;
import org.hibernate.Transaction;

import be.cmdb.builders.IdentityBuilder;
import be.cmdb.builders.LogBuilder;
import be.cmdb.dao.IdentityDAO;
import be.cmdb.dao.IdentityTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.Identity;
import be.cmdb.model.Log;
import be.cmdb.model.Type;

/**
 * Utility class for creating random Identity objects with fake data.
 */
public final class IdentityHelper {

    /**
     * This function will create a random Identity.
     * @param session The current hibernate session
     * @param typeId The TypeId of the Identity
     * @return The persisted Identity
     */
    public static Identity createRandomIdentity(Session session, Integer typeId) {
        IdentityTypeDAO dao = new IdentityTypeDAO();
        Type type = dao.findById(session, typeId);
        Identity identity = new IdentityBuilder()
            .withTypeId(type)
            .build();
        IdentityDAO identityDAO = new IdentityDAO();
        Transaction transaction = session.beginTransaction();
        identity = identityDAO.save(session, identity);
        String logText = "The Identity with Name: " + identity.getName()
            + " is created by Automation in table identity";
        Log log = new LogBuilder()
            .withIdentity(identity)
            .withLogText(logText)
            .build();
        LogDAO logDAO = new LogDAO();
        logDAO.save(session, log);
        transaction.commit();
        return identity;
    }

    /**
     * Creates a random Identity object with fake data.
     * @return Identity object
     */
    public static Identity createRandomIdentity(){
        return new IdentityBuilder().build();
    }

    /**
     * Creates a random Identity object and persists it to the database using the provided session.
     * @param session The current hibernate session
     * @param admin The Admin who is creating this Identity
     * @param active Whether the Identity should be active (1) or not (0)
     * @return Identity object that has been persisted to the database
     */
    public static Identity createRandomIdentity(Session session, Admin admin, boolean active) {
        Identity identity = new IdentityBuilder()
            .withLastModifiedAdminId(admin.getAdminId())
            .withActive(active ? 1 : 0)
            .build();
        IdentityTypeDAO identityTypeDAO = new IdentityTypeDAO();
        Type identityType = identityTypeDAO.findByType(session, "Werknemer");
        identity.setType(identityType);
        IdentityDAO identityDAO = new IdentityDAO();
        Transaction transaction = session.beginTransaction();
        identity = identityDAO.save(session, identity);
        // Log the creation of the Identity
        String logText = "The Identity with Name: " + identity.getName()
            + " is created by Automation in table identity";
        Log log = new LogBuilder()
            .withIdentity(identity)
            .withLogText(logText)
                .build();
        LogDAO logDAO = new LogDAO();
        logDAO.save(session, log);
        transaction.commit();
        return identity;
    }

    /**
     * This will delete an Identity object from the database.
     * @param session The current hibernate session
     * @param identity The Identity you want to delete
     */
    public static void deleteIdentity(Session session, Identity identity) {
        Transaction transaction = session.beginTransaction();
        IdentityDAO identityDAO = new IdentityDAO();

        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByIdentityId(session, identity.getIdenId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        identityDAO.deleteById(session, identity.getIdenId());
        transaction.commit();
    }
}
