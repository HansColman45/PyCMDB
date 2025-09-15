package be.cmdb.builders;

import be.cmdb.model.Device;

/**
 * Builder for creating a docking station device with default values.
 * Allows customization of specific fields through method chaining.
 */
public class DockingBuilder extends GenericBuilder<Device> {
    private final Device dockingStation;

    /**
     * Initializes a new DockingBuilder with default values for a docking station device.
     */
    public DockingBuilder() {
        this.dockingStation = new Device();
        dockingStation.setDiscriminator("docking");
        dockingStation.setActive(1); // Default to active
        dockingStation.setSerialNumber(getFaker().address().streetAddressNumber());
        dockingStation.setAssetTag("DOC" + getFaker().address().zipCode());
    }

    /**
     * Sets the Last Modified Admin ID for the docking station device.
     * @param lastModifiedAdminId The AdminId of the last modifier
     * @return the current DockingBuilder instance for method chaining
     */
    public DockingBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        dockingStation.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the Category ID for the docking station device.
     * @param categoryId The category ID to set
     * @return the current DockingBuilder instance for method chaining
     */
    public DockingBuilder withAssetCategoryId(int categoryId) {
        dockingStation.setCategoryId(categoryId);
        return this;
    }

    /**
     * Sets the Asset Type ID for the docking station device.
     * @param typeId The asset type ID to set
     * @return the current DockingBuilder instance for method chaining
     */
    public DockingBuilder withAssetTypeId(int typeId) {
        dockingStation.setTypeId(typeId);
        return this;
    }

    /**
     * Sets the Active status for the docking station device.
     * @param active The active status to set (1 for active, 0 for inactive)
     * @return the current DockingBuilder instance for method chaining
     */
    public DockingBuilder withActive(int active) {
        dockingStation.setActive(active);
        return this;
    }

    /**
     * Builds and returns the configured docking station device.
     * @return the constructed Device instance
     */
    @Override
    public Device build() {
        return this.dockingStation;
    }
}
