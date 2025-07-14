package be.cmdb.helpers;

import be.cmdb.dao.IdentityDAO;
import be.cmdb.dao.IdentityTypeDAO;
import be.cmdb.dao.LogDAO;
import be.cmdb.model.GeneralType;
import be.cmdb.model.Identity;
import be.cmdb.model.Log;
import com.github.javafaker.Faker;
import java.util.List;
import java.util.Locale;
import org.hibernate.Session;
import org.hibernate.Transaction;

/**
 * Utility class for creating random Identity objects with fake data.
 */
public final class IdentityHelper {

    /**
     * Creates a random Identity object with fake data.
     * @return a new Identity object populated with random data
     */
    public static Identity createRandomIdentity() {
        Faker faker = new Faker(Locale.UK);
        Identity identity = new Identity();
        identity.setName(faker.name().firstName() + ", " + faker.name().lastName());
        identity.setEmail(faker.internet().emailAddress());
        identity.setCompany(faker.company().name());
        identity.setUserId(faker.name().username());
        identity.setActive(1);
        identity.setLanguageCode("EN");
        return identity;
    }

    /**
     * This function will create a random Identity.
     * @param session The current hibernate session
     * @param typeId The TypeId of the Identity
     * @return The persisted Identity
     */
    public static Identity createRandomIdentity(Session session, Integer typeId) {
        Identity identity = createRandomIdentity();
        identity.setTypeId(typeId);
        IdentityDAO identityDAO = new IdentityDAO();
        Transaction transaction = session.beginTransaction();
        identityDAO.save(session, identity);
        transaction.commit();
        return identity;
    }

    /**
     * Creates a random Identity object and persists it to the database using the provided session.
     * @param session The current hibernate session
     * @return Identity object that has been persisted to the database
     */
    public static Identity createRandomIdentity(Session session) {
        Identity identity = createRandomIdentity();
        IdentityTypeDAO identityTypeDAO = new IdentityTypeDAO();
        GeneralType identityType = identityTypeDAO.findByType(session, "Werknemer");
        identity.setTypeId(identityType.GetTypeId());
        IdentityDAO identityDAO = new IdentityDAO();
        Transaction transaction = session.beginTransaction();
        identityDAO.save(session, identity);
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
