package be.cmdb.builders;

import be.cmdb.model.Type;

/**
 * Builder class for creating and configuring {@link Type} objects.
 * Provides fluent methods to set properties of the Type.
 */
public class AccountTypeBuilder extends GenericBuilder<Type> {
    /**
     * The Type object being built.
     */
    private final Type type;

    /**
     * Constructor to create a new AccountTypeBuilder instance.
     * Initializes a Type object with default values.
     */
    public AccountTypeBuilder(){
        this.type = new Type();
        type.setDiscriminator("AccountType");
        type.setActive(1); // Default to active
        type.setType(getFaker().commerce().productName());
        type.setDescription(getFaker().lorem().sentence());
    }

    /**
     * This will set the LastModifiedAdminId of the account type.
     * @param lastModifiedAdminId the AdminId of the admin who last modified the account type
     * @return The current builder instance.
     */
    public  AccountTypeBuilder withLastModifiedAdminId(int lastModifiedAdminId){
        type.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the account to active.
     * @param active 1 is active 0 is not.
     * @return The current builder instance.
     */
    public AccountTypeBuilder withActive(int active) {
        type.setActive(active);
        return this;
    }

    /**
     * Sets the type of the account.
     * @return Type The Type.
     */
    @Override
    public Type build() {
        return type;
    }
}
