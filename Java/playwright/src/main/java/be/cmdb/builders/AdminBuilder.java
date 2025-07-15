package be.cmdb.builders;

import be.cmdb.model.Admin;

import javax.xml.bind.DatatypeConverter;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.NoSuchProviderException;

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

    public AdminBuilder withLevel(int level) {
        admin.setLevel(level);
        return this;
    }

    public AdminBuilder withActive(int active) {
        admin.setActive(active);
        return this;
    }

    public AdminBuilder withAccountId(int accountId) {
        admin.setAccountId(accountId);
        return this;
    }

    public AdminBuilder withPassword(String password) {
        admin.setPassword(password);
        return this;
    }

    public Admin build() {
        return admin;
    }
}
