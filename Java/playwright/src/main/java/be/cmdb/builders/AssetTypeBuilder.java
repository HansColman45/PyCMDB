package be.cmdb.builders;

import be.cmdb.model.AssetType;

public class AssetTypeBuilder extends GenericBuilder<AssetType> {
    private final AssetType type;

    public AssetTypeBuilder(){
        this.type = new AssetType();
        type.setVendor(getFaker().commerce().material());
        type.setType(getFaker().commerce().productName());
        type.setActive(1);
    }

    public AssetTypeBuilder withCategoryId(int categoryId) {
        type.setCategoryId(categoryId);
        return this;
    }

    public AssetTypeBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        type.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public AssetTypeBuilder withActive(int active) {
        type.setActive(active);
        return this;
    }

    @Override
    public AssetType build() {
        return type;
    }
}
