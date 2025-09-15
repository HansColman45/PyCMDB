package be.cmdb.builders;

import be.cmdb.model.Device;

/**
 * Builder class for creating and configuring {@link Device} objects representing laptops.
 */
public class LaptopBuilder extends GenericBuilder<Device> {
    private final Device laptop;

    /**
     * Constructs a new {@code LaptopBuilder} and initializes the {@link Device} with default laptop properties.
     */
    public LaptopBuilder() {
        laptop = new Device();
        laptop.setActive(1); // Default to active
        laptop.setDiscriminator("laptop");
        laptop.setSerialNumber(getFaker().address().streetAddressNumber());
        laptop.setAssetTag("DST"+getFaker().address().zipCode());
        laptop.setRAM("128");
    }

    /**
     * Sets the lastModifiedAdminId for the {@link Device}.
     * @param lastModifiedAdminId The admin ID that last modified the laptop
     * @return this builder instance
     */
    public LaptopBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        laptop.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the categoryId for the {@link Device}.
     * @param categoryId the category ID to set
     * @return this builder instance
     */
    public LaptopBuilder withAssetCategoryId(int categoryId) {
        laptop.setCategoryId(categoryId);
        return this;
    }

    /**
     * Sets the typeId for the {@link Device}.
     * @param typeId the type ID to set
     * @return this builder instance
     */
    public LaptopBuilder withAssetTypeId(int typeId) {
        laptop.setTypeId(typeId);
        return this;
    }

    /**
     * Sets the active state for the {@link Device}.
     * @param active The active state to set (1 for active, 0 for inactive)
     * @return this builder instance
     */
    public LaptopBuilder withActive(int active) {
        laptop.setActive(active);
        return this;
    }

    /**
     * Sets the identityId for the {@link Device}.
     * @param identityId the identity ID to set
     * @return this builder instance
     */
    public LaptopBuilder withIdentityId(int identityId) {
        laptop.setIdentityId(identityId);
        return this;
    }

    /**
     * Builds and returns the configured {@link Device} object representing a laptop.
     * @return the configured Device object
     */
    @Override
    public Device build() {
        return this.laptop;
    }
}
