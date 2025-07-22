package be.cmdb.builders;

import be.cmdb.model.Admin;

import javax.xml.bind.DatatypeConverter;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
/**
 * Builder class for creating and configuring {@link Admin} objects.
 * Provides fluent methods to set properties of the Admin.
 */
public class AdminBuilder extends GenericBuilder {
    private final Admin admin;

    /**
     * Constructor for AdminBuilder.
     * Sets default values for the Admin object:
     */
    public AdminBuilder(){
        super();
        admin = new Admin();
        admin.setLevel(9);
        admin.setActive(1);
        try {
            MessageDigest md = MessageDigest.getInstance("MD5");
            md.update("1234".getBytes());
            byte[] digest = md.digest();
            admin.setPassword(DatatypeConverter
                .printHexBinary(digest).toUpperCase());
        }
        catch (NoSuchAlgorithmException ex)
        {
            throw new RuntimeException("MD5 algorithm not found", ex);
        }
    }

    /**
     * Sets the level for the Admin.
     * @param level the level of the admin (e.g., 1 for normal user, 9 for super admin)
     * @return AdminBuilder instance for method chaining
     */
    public AdminBuilder withLevel(int level) {
        admin.setLevel(level);
        return this;
    }

    /**
     * Sets the accountId for the Admin.
     * @param accountId the ID of the account associated with the admin
     * @return AdminBuilder instance for method chaining
     */
    public AdminBuilder withAccountId(int accountId) {
        admin.setAccountId(accountId);
        return this;
    }

    /**
     * Setts the lastModifiedAdminId for the Admin.
     * @param lastModifiedAdminId the ID of the admin who last modified this admin
     * @return AdminBuilder instance for method chaining
     */
    public AdminBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        admin.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the active status for the Admin.
     * @param active 1 for active, 0 for inactive
     * @return AdminBuilder instance for method chaining
     */
    public AdminBuilder withActive(int active) {
        admin.setActive(active);
        return this;
    }

    /**
     * Builds the Admin object with the specified properties.
     * @return Admin instance with the configured properties
     */
    public Admin build() {
        return admin;
    }
}
