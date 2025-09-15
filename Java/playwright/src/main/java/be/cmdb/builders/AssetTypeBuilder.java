package be.cmdb.builders;

import be.cmdb.model.AssetType;

/**
 * Builder class for creating and configuring {@link AssetType} objects.
 * Provides fluent methods to set properties of the AssetType.
 */
public class AssetTypeBuilder extends GenericBuilder<AssetType> {
    private final AssetType type;

    /**
     * Constructs a new {@code AssetTypeBuilder} and initializes the {@link AssetType}.
     */
    public AssetTypeBuilder(){
        this.type = new AssetType();
        type.setVendor(getFaker().commerce().material());
        type.setType(getFaker().commerce().productName());
        type.setActive(1);
    }

    /**
     * Sets the category ID for the {@link AssetType}.
     * @param categoryId the category ID to set
     * @return this builder instance
     */
    public AssetTypeBuilder withCategoryId(int categoryId) {
        type.setCategoryId(categoryId);
        return this;
    }

    /**
     * Sets the description for the {@link AssetType}.
     * @param lastModifiedAdminId the AdminId of the admin who last modified the asset type
     * @return this builder instance
     */
    public AssetTypeBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        type.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    /**
     * Sets the active status for the {@link AssetType}.
     * @param active 1 is active, 0 is not.
     * @return this builder instance
     */
    public AssetTypeBuilder withActive(int active) {
        type.setActive(active);
        return this;
    }

    /**
     * Builds and returns the configured {@link AssetType} object.
     * @return the built {@link AssetType}
     */
    @Override
    public AssetType build() {
        return type;
    }
}
