package be.cmdb.builders;

import be.cmdb.model.Device;

public class DesktopBuilder extends GenericBuilder<Device> {
    private final Device desktop;

    public DesktopBuilder(){
        this.desktop = new Device();
        desktop.setDiscriminator("desktop");
        desktop.setActive(1); // Default to active
        desktop.setSerialNumber(getFaker().address().streetAddressNumber());
        desktop.setAssetTag("DST"+getFaker().address().zipCode());
        desktop.setRAM("128");
    }

    public DesktopBuilder withLastModifiedAdminId(int lastModifiedAdminId) {
        desktop.setLastModifiedAdminId(lastModifiedAdminId);
        return this;
    }

    public DesktopBuilder withAssetCategoryId(int categoryId) {
        desktop.setCategoryId(categoryId);
        return this;
    }

    public DesktopBuilder withAssetTypeId(int typeId) {
        desktop.setTypeId(typeId);
        return this;
    }

    @Override
    public Device build() {
        return this.desktop;
    }
}
