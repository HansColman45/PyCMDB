package be.cmdb.builders;

import be.cmdb.model.Device;

/**
 * Builder for creating Token devices with default values.
 */
public class TokenBuilder extends GenericBuilder<Device> {
    private final Device token;

    /**
     * Constructor for TokenBuilder.
     * Initializes a new Device instance with discriminator set to "token"
     * and default values for serial number and asset tag.
     */
    public  TokenBuilder() {
        this.token = new Device();
        token.setDiscriminator("token");
        token.setActive(1); // Default to active
        token.setSerialNumber(getFaker().address().streetAddressNumber());
        token.setAssetTag("TOK" + getFaker().address().zipCode());
    }

    /**
     * Sets the last modified admin ID for the token.
     * @param lastModifiedAdminId The admin ID that last modified the token
     * @return this builder instance
     */
    public TokenBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        token.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the asset category ID for the token.
     * @param categoryId the category ID to set
     * @return this builder instance
     */
    public TokenBuilder withAssetCategoryId(int categoryId) {
        token.setCategoryId(categoryId);
        return this;
    }

    /**
     * Sets the asset type ID for the token.
     * @param typeId the type ID to set
     * @return this builder instance
     */
    public TokenBuilder withTypeId(int typeId) {
        token.setTypeId(typeId);
        return this;
    }

    /**
     * Sets the active state for the token.
     * @param active The active state to set (1 for active, 0 for inactive)
     * @return this builder instance
     */
    public TokenBuilder withActive(int active) {
        token.setActive(active);
        return this;
    }

    /**
     * Builds and returns the configured {@link Device} object representing the token.
     * @return the configured Device object
     */
    @Override
    public Device build() {
        return token;
    }
}
