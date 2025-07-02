package be.cmdb.helpers;

import java.util.Locale;

import org.hibernate.Session;

import com.github.javafaker.Faker;

import be.cmdb.model.Identity;

/**
 * Utility class for creating random Identity objects with fake data.
 */
public final class IdentityHelper {

    /**
    * Private constructor to prevent instantiation of the utility class.
    */
    private IdentityHelper() {
    }

    /**
     * Creates a random Identity object with fake data.
     * @return a new Identity object populated with random data
     */
    public static Identity createRandomIdentity() {
        Faker faker = new Faker(new Locale("en-GB"));
        Identity identity = new Identity();
        identity.setName(faker.name().firstName() + ", " + faker.name().lastName());
        identity.setEmail(faker.internet().emailAddress());
        identity.setCompany(faker.company().name());
        identity.setUserId(faker.name().username());
        identity.setActive(1);
        identity.setLanguageCode("EN");
        identity.setTypeId(4);
        return identity;
    }

    /**
     * Creates a random Identity object and persists it to the database using the provided session.
     * @param session
     * @return Identity object that has been persisted to the database
     */
    public static Identity createRandomIdentity(Session session) {
        Identity identity = createRandomIdentity();
        session.beginTransaction();
        session.persist(identity);
        session.getTransaction().commit();
        return identity;
    }
}
