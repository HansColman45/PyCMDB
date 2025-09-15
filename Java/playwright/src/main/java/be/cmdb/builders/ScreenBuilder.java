package be.cmdb.builders;

import be.cmdb.model.Device;

/**
 * Builder for creating Screen entities with default values.
 */
public class ScreenBuilder extends GenericBuilder<Device> {
    private final Device screen;

    /**
     * Constructor for ScreenBuilder.
     * Initializes a new Device instance with discriminator set to "screen"
     * and default values for serial number and asset tag.
     */
    public ScreenBuilder() {
        this.screen = new Device();
        screen.setDiscriminator("screen");
        screen.setActive(1); // Default to active
        screen.setSerialNumber(getFaker().address().streetAddressNumber());
        screen.setAssetTag("SCR" + getFaker().address().zipCode());
    }

    /**
     * Sets the last modified admin ID for the screen.
     * @param lastModifiedAdminId The admin ID that last modified the screen
     * @return this builder instance
     */
    public ScreenBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        screen.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the asset category ID for the screen.
     * @param categoryId the category ID to set
     * @return this builder instance
     */
    public ScreenBuilder withAssetCategoryId(int categoryId) {
        screen.setCategoryId(categoryId);
        return this;
    }

    /**
     * Sets the asset type ID for the screen.
     * @param typeId the type ID to set
     * @return this builder instance
     */
    public ScreenBuilder withTypeId(int typeId) {
        screen.setTypeId(typeId);
        return this;
    }

    /**
     * Sets the active state for the screen.
     * @param active The active state to set (1 for active, 0 for inactive)
     * @return this builder instance
     */
    public ScreenBuilder withActive(int active) {
        screen.setActive(active);
        return this;
    }

    /**
     * Builds and returns the configured screen entity.
     * @return the configured screen entity
     */
    @Override
    public Device build() {
        return this.screen;
    }
}
