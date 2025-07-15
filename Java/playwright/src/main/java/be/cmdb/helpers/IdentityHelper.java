package be.cmdb.helpers;

import be.cmdb.builders.IdentityBuilder;
import be.cmdb.builders.LogBuilder;
import be.cmdb.dao.IdentityDAO;
import be.cmdb.dao.IdentityTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.GeneralType;
import be.cmdb.model.Identity;
import be.cmdb.model.Log;
import java.util.List;
import org.hibernate.Session;
import org.hibernate.Transaction;

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
        Identity identity = new IdentityBuilder()
            .withTypeId(typeId)
            .build();
        IdentityDAO identityDAO = new IdentityDAO();
        Transaction transaction = session.beginTransaction();
        identity = identityDAO.save(session, identity);
        String logText = "The Identity with Name: " + identity.getName()
            + " is created by Automation in table identity";
        Log log = new LogBuilder()
            .withIdentityId(identity.getIdenId())
            .withLogText(logText)
            .build();
        LogDAO logDAO = new LogDAO();
        logDAO.save(session, log);
        transaction.commit();
        return identity;
    }

    /**
     * Creates a random Identity object and persists it to the database using the provided session.
     * @param session The current hibernate session
     * @return Identity object that has been persisted to the database
     */
    public static Identity createRandomIdentity(Session session) {
        Identity identity = new IdentityBuilder().build();
        IdentityTypeDAO identityTypeDAO = new IdentityTypeDAO();
        GeneralType identityType = identityTypeDAO.findByType(session, "Werknemer");
        identity.setTypeId(identityType.getTypeId());
        IdentityDAO identityDAO = new IdentityDAO();
        Transaction transaction = session.beginTransaction();
        identity = identityDAO.save(session, identity);
        // Log the creation of the Identity
        String logText = "The Identity with Name: " + identity.getName()
            + " is created by Automation in table identity";
        Log log = new LogBuilder()
            .withIdentityId(identity.getIdenId())
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
    public void deleteIdentity(Session session, Identity identity) {
        Transaction transaction = session.beginTransaction();
        IdentityDAO identityDAO = new IdentityDAO();
        LogDAO logDAO = new LogDAO();
        List<Log> logs = logDAO.findByIdentityId(session, identity.getIdenId());
        for (Log log : logs) {
            logDAO.delete(session, log);
        }
        identityDAO.delete(session, identity);
        transaction.commit();
    }
}
