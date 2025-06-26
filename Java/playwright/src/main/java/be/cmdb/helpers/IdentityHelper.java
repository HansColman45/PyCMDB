package be.cmdb.helpers;
import com.github.javafaker.Faker;

import be.cmdb.model.Identity;

public class IdentityHelper {
    private static final Faker faker = new Faker();
    private IdentityHelper() {
        // Private constructor to prevent instantiation
    }
    
    public static Identity createRandomIdentity() {
        Identity identity = new Identity();
        identity.setFirstName(faker.name().firstName());
        identity.setLastName(faker.name().lastName());
        identity.setEmail(faker.internet().emailAddress());
        identity.setCompany(faker.company().name());
        identity.setUserId(faker.name().username());
        return identity;
    }
}
