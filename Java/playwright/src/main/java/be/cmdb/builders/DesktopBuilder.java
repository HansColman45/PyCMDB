package be.cmdb.builders;

import be.cmdb.model.Device;

/**
 * Builder for creating a desktop device with default values.
 * Allows customization of specific fields through method chaining.
 */
public class DesktopBuilder extends GenericBuilder<Device> {
    private final Device desktop;

    /**
     * Initializes a new DesktopBuilder with default values for a desktop device.
     */
    public DesktopBuilder(){
        this.desktop = new Device();
        desktop.setDiscriminator("desktop");
        desktop.setActive(1); // Default to active
        desktop.setSerialNumber(getFaker().address().streetAddressNumber());
        desktop.setAssetTag("DST"+getFaker().address().zipCode());
        desktop.setRAM("128");
    }

    /**
     * Sets the Last Modified Admin ID for the desktop device.
     * @param lastModifiedAdminId The AdminId of the last modifier
     * @return the current DesktopBuilder instance for method chaining
     */
    public DesktopBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        desktop.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the Category ID for the desktop device.
     * @param categoryId The category ID to set
     * @return the current DesktopBuilder instance for method chaining
     */
    public DesktopBuilder withAssetCategoryId(int categoryId) {
        desktop.setCategoryId(categoryId);
        return this;
    }

    /**
     * Sets the Asset Type ID for the desktop device.
     * @param typeId The asset type ID to set
     * @return the current DesktopBuilder instance for method chaining
     */
    public DesktopBuilder withAssetTypeId(int typeId) {
        desktop.setTypeId(typeId);
        return this;
    }

    /**
     * Sets the Active status for the desktop device.
     * @param active The active status to set (1 for active, 0 for inactive)
     * @return the current DesktopBuilder instance for method chaining
     */
    public DesktopBuilder withActive(int active) {
        desktop.setActive(active);
        return this;
    }

    /**
     * Builds and returns the configured desktop device.
     * @return the current Device instance
     */
    @Override
    public Device build() {
        return this.desktop;
    }
}
