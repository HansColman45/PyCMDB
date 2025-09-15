package be.cmdb.builders;

import be.cmdb.model.Type;

/**
 * Builder class for creating and configuring {@link Type} objects.
 * Provides fluent methods to set properties of the Type.
 */
public class IdentityTypeBuilder extends GenericBuilder<Type> {
    private final Type type;

    /**
     * Constructs a new {@code IdentityTypeBuilder} and initializes the {@link Type}.
     */
    public IdentityTypeBuilder() {
        this.type = new Type();
        type.setDiscriminator("IdentityType");
        type.setActive(1); // Default to active
        type.setType(getFaker().commerce().productName());
        type.setDescription(getFaker().lorem().sentence());
    }

    /**
     * Sets the last modified admin ID for the {@link Type}.
     * @param lastModifiedAdminId the AdminId of the admin who last modified the identity type
     * @return this builder instance
     */
    public IdentityTypeBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        type.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the active status for the {@link Type}.
     * @param active 1 is active, 0 is not.
     * @return this builder instance
     */
    public IdentityTypeBuilder withActive(int active) {
        type.setActive(active);
        return this;
    }

    /**
     * Builds and returns the configured {@link Type} object.
     * @return the built {@link Type}
     */
    @Override
    public Type build() {
        return type;
    }
}
