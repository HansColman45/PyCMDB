package be.cmdb.builders;

import be.cmdb.model.Identity;

/**
 * Builder class for creating and configuring {@link Identity} objects.
 * Provides fluent methods to set properties of the Identity.
 */
public class IdentityBuilder extends GenericBuilder<Identity> {
    private final Identity identity;

    /**
     * Constructs a new {@code IdentityBuilder} and initializes the {@link Identity}.
     */
    public IdentityBuilder() {
        super();
        identity = new Identity();
        identity.setName(getFaker().name().firstName() + ", " + getFaker().name().lastName());
        identity.setEmail(getFaker().internet().emailAddress());
        identity.setCompany(getFaker().company().name());
        identity.setUserId(getFaker().name().username());
        identity.setActive(1);
        identity.setLanguageCode("EN");
    }

    /**
     * Sets the type ID for the {@link Identity}.
     * @param typeId the type ID to set
     * @return this builder instance
     */
    public IdentityBuilder withTypeId(Integer typeId) {
        identity.setTypeId(typeId);
        return this;
    }

    /**
     * Sets the language code for the {@link Identity}.
     * @param languageCode the language code to set
     * @return this builder instance
     */
    public IdentityBuilder withLanguageCode(String languageCode) {
        identity.setLanguageCode(languageCode);
        return this;
    }

    /**
     * Sets the name for the {@link Identity}.
     * @param name the name to set
     * @return this builder instance
     */
    public IdentityBuilder withName(String name) {
        identity.setName(name);
        return this;
    }

    /**
     * Sets the email for the {@link Identity}.
     * @param email the email to set
     * @return this builder instance
     */
    public IdentityBuilder withEmail(String email) {
        identity.setEmail(email);
        return this;
    }

    /**
     * Sets the userId for the {@link Identity}.
     * @param userId the userId to set
     * @return this builder instance
     */
    public IdentityBuilder withUserId(String userId) {
        identity.setUserId(userId);
        return this;
    }

    public IdentityBuilder withLastModifiedAdminId(Integer adminId) {
        identity.setLastModifiedAdminId(adminId);
        return this;
    }

    /**
     * Builds and returns the configured {@link Identity} object.
     * @return the built {@link Identity}
     */
    public Identity build() {
        return identity;
    }
}
