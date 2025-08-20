package be.cmdb.builders;

import be.cmdb.model.Device;

public class LaptopBuilder extends GenericBuilder<Device> {
    private final Device laptop;

    public LaptopBuilder() {
        laptop = new Device();
        laptop.setActive(1); // Default to active
        laptop.setDiscriminator("laptop");
        laptop.setSerialNumber(getFaker().address().streetAddressNumber());
        laptop.setAssetTag("DST"+getFaker().address().zipCode());
        laptop.setRAM("128");
    }

    public LaptopBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        laptop.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public LaptopBuilder withAssetCategoryId(int categoryId) {
        laptop.setCategoryId(categoryId);
        return this;
    }

    public LaptopBuilder withAssetTypeId(int typeId) {
        laptop.setTypeId(typeId);
        return this;
    }

    public LaptopBuilder withActive(int active) {
        laptop.setActive(active);
        return this;
    }

    public LaptopBuilder withIdentityId(int identityId) {
        laptop.setIdentityId(identityId);
        return this;
    }

    @Override
    public Device build() {
        return this.laptop;
    }
}
